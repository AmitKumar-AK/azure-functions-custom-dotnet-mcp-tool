using System.Net;
using EnterpriseMcpServer.Models;
using EnterpriseMcpServer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace EnterpriseMcpServer.Functions.McpTools;

public sealed class McpCreateEnquiryTool
{
    private readonly IDataverseService _dataverseService;
    private readonly ILogger<McpCreateEnquiryTool> _logger;

    public McpCreateEnquiryTool(IDataverseService dataverseService, ILogger<McpCreateEnquiryTool> logger)
    {
        _dataverseService = dataverseService;
        _logger = logger;
    }

    [Function(nameof(CreateEnquiryTool))]
    public async Task<string> CreateEnquiryTool(
        [McpToolTrigger(
            "create_enquiry",
            "Creates a new customer enquiry record in Microsoft Dataverse.")]
        ToolInvocationContext context,
        [McpToolProperty("fullName", "The full name of the customer submitting the enquiry.", isRequired: true)]
        string fullName,
        [McpToolProperty("email", "Customer valid email address.", isRequired: true)]
        string email,
        [McpToolProperty("message", "The enquiry message or requirements body.", isRequired: true)]
        string message,
        [McpToolProperty("source", "The origin of the lead (e.g., 'SitecoreAI Form', 'AI Agent').", isRequired: false)]
        string? source = "MCP Agent",
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("MCP Tool 'create_enquiry' invoked for customer: {Email}", email);

        var request = new CreateEnquiryRequest
        {
            Name = fullName,
            Email = email,
            Message = message,
            Source = source ?? "MCP Agent"
        };

        var recordId = await _dataverseService.CreateEnquiryAsync(request, cancellationToken);

        return System.Text.Json.JsonSerializer.Serialize(new
        {
            success = true,
            message = "Enquiry created successfully in Dataverse.",
            recordId = recordId.ToString()
        });
    }
}