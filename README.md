# Azure Functions Custom Dotnet MCP Tool

> Build, deploy, and scale custom **Model Context Protocol (MCP)** tools using **C# .NET Isolated Azure Functions** with the `McpToolTrigger` binding.

This project demonstrates how to build enterprise-grade MCP tools as Azure Functions with proper dependency injection, input validation, error handling, and logging - connecting to real enterprise backends including **Microsoft Dataverse**, a **remote JSON feed**, and **Sitecore XM Cloud Edge GraphQL**.

[![SitecoreAI](https://img.shields.io/badge/SitecoreAI-XM%20Cloud-red?logo=sitecore)](https://doc.sitecore.com/sai)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![Azure Functions](https://img.shields.io/badge/Azure-Functions-0078D4)](https://azure.microsoft.com/en-us/services/functions/)
[![Microsoft Dataverse](https://img.shields.io/badge/Microsoft-Dataverse-742774)](https://powerplatform.microsoft.com/en-us/dataverse/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

## Features

- 6 MCP tools exposed as Azure Functions using `McpToolTrigger`
- Microsoft Dataverse integration via `Microsoft.PowerPlatform.Dataverse.Client`
- Sitecore XM Cloud Edge GraphQL integration
- Remote JSON feed integration via `IHttpClientFactory`
- Application Insights telemetry
- Full dependency injection with scoped service lifetimes
- Input validation and structured JSON responses

---

## MCP Tools

### Enquiry Management (Microsoft Dataverse)

| Tool | Description |
|------|-------------|
| `create_enquiry` | Creates a new customer enquiry record in Dataverse |
| `get_recent_enquiries` | Retrieves recent enquiry records (paginated, max 100) |
| `get_enquiry_by_id` | Fetches a single enquiry record by its Dataverse GUID |

**`create_enquiry` parameters:**
- `fullName` *(required)* - Full name of the customer
- `email` *(required)* - Customer email address
- `message` *(required)* - Enquiry message body
- `source` *(optional)* - Origin of the lead (default: `"MCP Agent"`)

**`get_recent_enquiries` parameters:**
- `pageSize` *(optional)* - Number of records to return (1–100, default: `10`)

**`get_enquiry_by_id` parameters:**
- `enquiryId` *(required)* - GUID of the Dataverse record

---

### Monkey Species Data (Remote JSON Feed)

| Tool | Description |
|------|-------------|
| `get_monkeys` | Retrieves all monkey species records from a remote JSON feed |
| `get_monkey_by_name` | Fetches a single monkey record by name (case-insensitive) |

**`get_monkey_by_name` parameters:**
- `name` *(required)* - Name of the monkey species

---

### Sitecore Content (Edge GraphQL)

| Tool | Description |
|------|-------------|
| `get_sitecore_item` | Fetches a Sitecore item and its children by path and language |

**`get_sitecore_item` parameters:**
- `path` *(required)* - Sitecore item path or item ID (e.g. `/sitecore/content/Home`)
- `language` *(optional)* - Language version to retrieve (default: `en`)

---

## Project Structure

    .
    ├── Functions/ # MCP tool trigger functions (one file per tool)
    ├── Models/ # Request/response model classes
    ├── Services/ # Business logic + external API integrations
    ├── Program.cs # DI setup and host configuration
    ├── Constants.cs # Shared constant values
    ├── host.json # Azure Functions host configuration
    └── local.settings.json # Local environment variables (not committed)`


---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Azure Functions Core Tools v4](https://learn.microsoft.com/azure/azure-functions/functions-run-local)
- [Azurite](https://learn.microsoft.com/azure/storage/common/storage-use-azurite) (local storage emulator) or an Azure Storage account
- A Microsoft Dataverse environment with a registered app (Client ID + Secret)
- *(Optional)* A SitecoreAI (XM Cloud) tenant with an Edge API key
- *(Optional)* An Application Insights resource

---

## Configuration

Copy `local.settings.json` and fill in your values:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "APPLICATIONINSIGHTS_CONNECTION_STRING": "YOUR-APPINSIGHTS-CONNECTION-STRING",
    "DataverseEnvironment": "YOUR-DATAVERSE-ENVIRONMENT-URL",
    "ClientId": "YOUR-AAD-CLIENT-ID",
    "ClientSecret": "YOUR-AAD-CLIENT-SECRET",
    "MonkeysUrl": "YOUR-MONKEYS-JSON-FEED-URL",
    "EdgeGQLUrl": "https://edge.sitecorecloud.io/api/graphql/v1",
    "EdgeGQLKey": "YOUR-SITECORE-EDGE-API-KEY"
  }
}
```

| Key | Requried | Description |
|------|-------------|-------------|
| `DataverseEnvironment` | Yes (Dataverse tools) | Base URL of your Dataverse environment |
| `ClientId` | Yes (Dataverse tools) | Azure AD app registration Client ID |
| `ClientSecret` | Yes (Dataverse tools) | Azure AD app registration Client Secret |
| `MonkeysUrl` | Yes (Monkey tools) | URL of the remote monkey species JSON feed |
| `EdgeGQLUrl` | Yes (Sitecore tool) | Sitecore Edge GraphQL endpoint |
| `EdgeGQLKey` | Yes (Sitecore tool) | Sitecore Edge API key (`sc_apikey`) |
| `APPLICATIONINSIGHTS_CONNECTION_STRING` | No | Application Insights telemetry |


### Run Locally
```
cd EnterpriseMcpServer
func start
```

The MCP server will start and expose all tools via the Azure Functions runtime. Connect your MCP client (e.g. VS Code with GitHub Copilot) to `http://localhost:7071/runtime/webhooks/mcp/sse`.

### Deploy to Azure

Deploy as a standard Azure Functions app (consumption, premium, or dedicated plan). Set all configuration values from the table above as Application Settings in the Azure portal or via Azure CLI:

```
az functionapp config appsettings set \
  --name <your-function-app> \
  --resource-group <your-rg> \
  --settings DataverseEnvironment="..." ClientId="..." ClientSecret="..."
```

## Key NuGet Packages
| Package | Version | Purpose |
|------|-------------|-------------|
| `Microsoft.Azure.Functions.Worker` | 2.51.0 | Core Azure Functions isolated runtime |
| `Microsoft.Azure.Functions.Worker.Extensions.Mcp` | 1.6.0 | `McpToolTrigger` binding support |
| `Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore` | 2.1.0 | HTTP trigger with ASP.NET Core |
| `Microsoft.PowerPlatform.Dataverse.Client` | 1.2.26 | Dataverse SDK |
| `Microsoft.Azure.Functions.Worker.ApplicationInsights` | 2.50.0 |Application Insights integration |


## License

MIT

### Keywords for Developers

`SitecoreAI` `Sitecore XM Cloud` `Microsoft Dataverse` `Azure Functions` `Power Platform` `Dynamics 365` `Certificate Authentication` `Serverless Integration` `.NET 8` `C#` `MSFD Developer Environment` `Headless CMS` `Form Submission` `CRUD Operations` `Azure AD` `OAuth 2.0` `Production Ready`

---

**⭐ If this repository helped you, please give it a star!**


> Built with ❤️ for the Sitecore developer community. If this project helped you, consider sharing it or contributing back - every star and PR supports the community.
