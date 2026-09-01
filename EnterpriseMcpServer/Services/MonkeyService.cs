using EnterpriseMcpServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace EnterpriseMcpServer.Services;

/// <summary>
/// Service implementation for retrieving monkey records from the remote JSON feed.
/// </summary>
public sealed class MonkeyService : IMonkeyService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MonkeyService> _logger;

    public MonkeyService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<MonkeyService> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all monkey records from the remote JSON feed.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A collection of monkey responses.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the remote feed cannot be reached or returns an error.</exception>
    public async Task<IEnumerable<MonkeyResponse>> GetMonkeysAsync(CancellationToken cancellationToken = default)
    {
        var monkeysUrl = _configuration["MonkeysUrl"]
            ?? throw new InvalidOperationException("MonkeysUrl configuration value is missing.");

        var client = _httpClientFactory.CreateClient();

        var monkeys = await client.GetFromJsonAsync<List<MonkeyResponse>>(monkeysUrl, cancellationToken);

        if (monkeys is null)
        {
            _logger.LogWarning("Monkey feed returned null response from {Url}", monkeysUrl);
            return [];
        }

        _logger.LogInformation("Retrieved {Count} monkey records from remote feed.", monkeys.Count);
        return monkeys;
    }

    /// <summary>
    /// Retrieves a single monkey record by name (case-insensitive match).
    /// </summary>
    /// <param name="name">The name of the monkey to find.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The monkey response if found; otherwise, null.</returns>
    public async Task<MonkeyResponse?> GetMonkeyByNameAsync(string name, CancellationToken cancellationToken)
    {
        var monkeys = await GetMonkeysAsync(cancellationToken);

        var monkey = monkeys.FirstOrDefault(m =>
            m.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);

        if (monkey is null)
        {
            _logger.LogWarning("Monkey with name '{Name}' was not found.", name);
        }
        else
        {
            _logger.LogInformation("Retrieved monkey record for '{Name}'.", name);
        }

        return monkey;
    }
}
