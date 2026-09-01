using EnterpriseMcpServer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EnterpriseMcpServer.Functions.McpTools;

public sealed class McpGetMonkeysTool
{
    private readonly IMonkeyService _monkeyService;
    private readonly ILogger<McpGetMonkeysTool> _logger;

    public McpGetMonkeysTool(IMonkeyService monkeyService, ILogger<McpGetMonkeysTool> logger)
    {
        _monkeyService = monkeyService;
        _logger = logger;
    }

    [Function(nameof(GetMonkeysTool))]
    public async Task<string> GetMonkeysTool(
        [McpToolTrigger(
            "get_monkeys",
            "Retrieves a list of all monkey records from the remote JSON feed.")]
        ToolInvocationContext context,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("MCP Tool 'get_monkeys' invoked.");

        var monkeys = await _monkeyService.GetMonkeysAsync(cancellationToken);
        return JsonSerializer.Serialize(monkeys);
    }
}
