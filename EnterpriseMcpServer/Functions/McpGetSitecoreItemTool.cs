using EnterpriseMcpServer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EnterpriseMcpServer.Functions.McpTools;

public sealed class McpGetSitecoreItemTool
{
    private readonly ISitecoreAIService _sitecoreAIService;
    private readonly ILogger<McpGetSitecoreItemTool> _logger;

    public McpGetSitecoreItemTool(ISitecoreAIService sitecoreAIService, ILogger<McpGetSitecoreItemTool> logger)
    {
        _sitecoreAIService = sitecoreAIService;
        _logger = logger;
    }

    [Function(nameof(GetSitecoreItemTool))]
    public async Task<string> GetSitecoreItemTool(
        [McpToolTrigger(
            "get_sitecore_item",
            "Fetches a Sitecore item and its children by path and language using the Edge GraphQL API.")]
        ToolInvocationContext context,
        [McpToolProperty("path", "The Sitecore item path or item ID (e.g. '/sitecore/content/Home').", isRequired: true)]
        string path,
        [McpToolProperty("language", "The language version to retrieve (e.g. 'en'). Defaults to 'en'.", isRequired: false)]
        string language = "en",
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("MCP Tool 'get_sitecore_item' invoked for path: {Path}, language: {Language}", path, language);

        var item = await _sitecoreAIService.GetItemChildDetailsAsync(path, language, cancellationToken);

        if (item is null)
        {
            return JsonSerializer.Serialize(new { error = $"Sitecore item not found for path '{path}' and language '{language}'." });
        }

        return JsonSerializer.Serialize(item);
    }
}
