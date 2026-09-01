using EnterpriseMcpServer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnterpriseMcpServer.Functions.McpTools
{
    public sealed class McpGetEnquiriesTool
    {
        private readonly IDataverseService _dataverseService;
        private readonly ILogger<McpGetEnquiriesTool> _logger;

        public McpGetEnquiriesTool(IDataverseService dataverseService, ILogger<McpGetEnquiriesTool> logger)
        {
            _dataverseService = dataverseService;
            _logger = logger;
        }

        [Function(nameof(GetRecentEnquiriesTool))]
        public async Task<string> GetRecentEnquiriesTool(
            [McpToolTrigger(
            "get_recent_enquiries",
            "Retrieves a list of recent customer enquiry records from Dataverse.")]
        ToolInvocationContext context,
            [McpToolProperty("pageSize", "Number of enquiry records to return (1-100). Default is 10.", isRequired: false)]
        int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var clampedSize = Math.Clamp(pageSize, 1, 100);
            _logger.LogInformation("MCP Tool 'get_recent_enquiries' invoked with pageSize: {Size}", clampedSize);

            var enquiries = await _dataverseService.GetEnquiriesAsync(clampedSize, cancellationToken);
            return System.Text.Json.JsonSerializer.Serialize(enquiries);
        }
    }
}