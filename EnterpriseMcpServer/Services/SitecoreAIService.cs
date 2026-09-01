using EnterpriseMcpServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EnterpriseMcpServer.Services;

/// <summary>
/// Service implementation for fetching Sitecore item details via the Edge GraphQL API.
/// </summary>
public sealed class SitecoreAIService : ISitecoreAIService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SitecoreAIService> _logger;

    public SitecoreAIService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<SitecoreAIService> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Fetches a Sitecore item and its children by path and language via Edge GraphQL.
    /// </summary>
    /// <param name="path">The Sitecore item path or item ID.</param>
    /// <param name="language">The language version to retrieve (e.g. "en").</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The item response if found; otherwise, null.</returns>
    /// <exception cref="InvalidOperationException">Thrown when configuration values are missing.</exception>
    public async Task<SitecoreItemResponse?> GetItemChildDetailsAsync(
        string path,
        string language,
        CancellationToken cancellationToken = default)
    {
        var edgeGQLUrl = _configuration["EdgeGQLUrl"]
            ?? throw new InvalidOperationException("EdgeGQLUrl configuration value is missing.");

        var edgeGQLKey = _configuration["EdgeGQLKey"]
            ?? throw new InvalidOperationException("EdgeGQLKey configuration value is missing.");

        var query = $@"
            query Item {{
                item(language: ""{language}"", path: ""{path}"") {{
                    id
                    name
                    path
                    displayName
                    hasChildren
                    children {{
                        results {{
                            id
                            name
                            path
                        }}
                    }}
                }}
            }}";

        var requestBody = JsonSerializer.Serialize(new { query, variables = new { } });

        var request = new HttpRequestMessage(HttpMethod.Post, edgeGQLUrl)
        {
            Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("sc_apikey", edgeGQLKey);

        var client = _httpClientFactory.CreateClient();
        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var graphQLResponse = await response.Content
            .ReadFromJsonAsync<GraphQLResponse<SitecoreItemData>>(cancellationToken: cancellationToken);

        var item = graphQLResponse?.Data?.Item;

        if (item is null)
        {
            _logger.LogWarning("Sitecore item not found for path '{Path}' and language '{Language}'.", path, language);
            return null;
        }

        _logger.LogInformation("Retrieved Sitecore item '{Name}' at path '{Path}'.", item.Name, item.Path);
        return item;
    }

    // Internal GraphQL response wrappers — not exposed outside this service
    private sealed class GraphQLResponse<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }

    private sealed class SitecoreItemData
    {
        [JsonPropertyName("item")]
        public SitecoreItemResponse? Item { get; set; }
    }
}
