using EnterpriseMcpServer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EnterpriseMcpServer.Functions.McpTools;

public sealed class McpGetMonkeyByNameTool
{
    private readonly IMonkeyService _monkeyService;
    private readonly ILogger<McpGetMonkeyByNameTool> _logger;

    public McpGetMonkeyByNameTool(IMonkeyService monkeyService, ILogger<McpGetMonkeyByNameTool> logger)
    {
        _monkeyService = monkeyService;
        _logger = logger;
    }

    [Function(nameof(GetMonkeyByNameTool))]
    public async Task<string> GetMonkeyByNameTool(
        [McpToolTrigger(
            "get_monkey_by_name",
            "Fetches a single monkey record by its name.")]
        ToolInvocationContext context,
        [McpToolProperty("name", "The name of the monkey to retrieve.", isRequired: true)]
        string name,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("MCP Tool 'get_monkey_by_name' invoked with name: {Name}", name);

        var monkey = await _monkeyService.GetMonkeyByNameAsync(name, cancellationToken);
        if (monkey is null)
        {
            return JsonSerializer.Serialize(new { error = $"Monkey with name '{name}' not found." });
        }

        return JsonSerializer.Serialize(monkey);
    }
}
