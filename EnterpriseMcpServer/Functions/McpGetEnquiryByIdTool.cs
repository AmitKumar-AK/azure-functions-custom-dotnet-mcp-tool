using EnterpriseMcpServer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace EnterpriseMcpServer.Functions.McpTools;

public sealed class McpGetEnquiryByIdTool
{
    private readonly IDataverseService _dataverseService;
    private readonly ILogger<McpGetEnquiryByIdTool> _logger;

    public McpGetEnquiryByIdTool(IDataverseService dataverseService, ILogger<McpGetEnquiryByIdTool> logger)
    {
        _dataverseService = dataverseService;
        _logger = logger;
    }

    [Function(nameof(GetEnquiryByIdTool))]
    public async Task<string> GetEnquiryByIdTool(
        [McpToolTrigger(
            "get_enquiry_by_id",
            "Fetches a single enquiry record by its unique Dataverse GUID.")]
        ToolInvocationContext context,
        [McpToolProperty("enquiryId", "The GUID of the Dataverse enquiry record.", isRequired: true)]
        string enquiryId,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(enquiryId, out var idGuid))
        {
            return System.Text.Json.JsonSerializer.Serialize(new { error = "Invalid GUID format." });
        }

        var enquiry = await _dataverseService.GetEnquiryByIdAsync(idGuid, cancellationToken);
        if (enquiry is null)
        {
            return System.Text.Json.JsonSerializer.Serialize(new { error = $"Record with ID {enquiryId} not found." });
        }

        return System.Text.Json.JsonSerializer.Serialize(enquiry);
    }
}