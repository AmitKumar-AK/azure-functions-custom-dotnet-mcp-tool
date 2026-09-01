using EnterpriseMcpServer.Models;
using EnterpriseMcpServer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseMcpServer.Services;

/// <summary>
/// Service implementation for managing enquiry records in Microsoft Dataverse.
/// Provides CRUD operations, validation, and data mapping for enquiry entities.
/// </summary>
public sealed class DataverseService : IDataverseService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DataverseService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataverseService"/> class.
    /// </summary>
    /// <param name="configuration">Application configuration containing Dataverse connection settings.</param>
    /// <param name="logger">Logger for tracking service operations and errors.</param>
    public DataverseService(
        IConfiguration configuration,
        ILogger<DataverseService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }


    /// <summary>
    /// Creates a new enquiry record in Dataverse.
    /// Validates field lengths and truncates values if necessary before creating the entity.
    /// </summary>
    /// <param name="request">The request containing enquiry details to create.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The unique identifier (GUID) of the newly created enquiry.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails or field lengths exceed maximum allowed values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when unable to connect to Dataverse.</exception>
    public async Task<Guid> CreateEnquiryAsync(
        CreateEnquiryRequest request,
        CancellationToken cancellationToken)
    {
        // Validate field lengths before attempting to create
        ValidateFieldLengths(request);

        using var client = CreateServiceClient();

        var entity = new Entity(Constants.Dataverse.TableName)
        {
            [Constants.Dataverse.Columns.NewColumn] = TruncateIfNeeded(request.Name, Constants.Dataverse.MaxLength.Name),
            [Constants.Dataverse.Columns.Email] = TruncateIfNeeded(request.Email, Constants.Dataverse.MaxLength.Email),
            [Constants.Dataverse.Columns.Message] = TruncateIfNeeded(request.Message, Constants.Dataverse.MaxLength.Message),
            [Constants.Dataverse.Columns.Source] = TruncateIfNeeded(request.Source ?? Constants.Dataverse.DefaultSource, Constants.Dataverse.MaxLength.Source)
        };

        var id = await Task.Run(() => client.Create(entity), cancellationToken);

        _logger.LogInformation(
            Constants.LogMessages.CreatedDataverseEnquiryRecord,
            id);

        return id;
    }


    /// <summary>
    /// Retrieves multiple enquiries from Dataverse, ordered by creation date descending.
    /// </summary>
    /// <param name="topCount">The maximum number of enquiries to retrieve. Default is 100.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A collection of enquiry responses.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to connect to Dataverse.</exception>
    public async Task<IEnumerable<EnquiryResponse>> GetEnquiriesAsync(
        int topCount = 100,
        CancellationToken cancellationToken = default)
    {
        using var client = CreateServiceClient();

        var query = new QueryExpression(Constants.Dataverse.TableName)
        {
            ColumnSet = new ColumnSet(
                Constants.Dataverse.Columns.NewColumn,
                Constants.Dataverse.Columns.Email,
                Constants.Dataverse.Columns.Message,
                Constants.Dataverse.Columns.Source,
                Constants.Dataverse.Columns.CreatedOn,
                Constants.Dataverse.Columns.ModifiedOn),
            TopCount = topCount
        };

        query.AddOrder(Constants.Dataverse.Columns.CreatedOn, OrderType.Descending);

        var results = await Task.Run(() =>
            client.RetrieveMultiple(query),
            cancellationToken);

        _logger.LogInformation(
            Constants.LogMessages.RetrievedEnquiryRecords,
            results.Entities.Count);

        return results.Entities.Select(MapEntityToResponse).ToList();
    }


    /// <summary>
    /// Retrieves a single enquiry by its unique identifier.
    /// Returns null if the enquiry is not found.
    /// </summary>
    /// <param name="id">The unique identifier of the enquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The enquiry response if found; otherwise, null.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to connect to Dataverse.</exception>
    public async Task<EnquiryResponse?> GetEnquiryByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        using var client = CreateServiceClient();

        var entity = await Task.Run(() =>
            client.Retrieve(
                Constants.Dataverse.TableName,
                id,
                new ColumnSet(
                    Constants.Dataverse.Columns.NewColumn,
                    Constants.Dataverse.Columns.Email,
                    Constants.Dataverse.Columns.Message,
                    Constants.Dataverse.Columns.Source,
                    Constants.Dataverse.Columns.CreatedOn,
                    Constants.Dataverse.Columns.ModifiedOn)
            ),
            cancellationToken);

        if (entity == null)
        {
            _logger.LogWarning(
                Constants.LogMessages.EnquiryRecordNotFound,
                id);
            return null;
        }

        _logger.LogInformation(
            Constants.LogMessages.RetrievedDataverseEnquiryRecord,
            id);

        return MapEntityToResponse(entity);
    }


    /// <summary>
    /// Creates and initializes a ServiceClient for connecting to Microsoft Dataverse.
    /// </summary>
    /// <returns>A configured ServiceClient instance ready to interact with Dataverse.</returns>
    /// <exception cref="InvalidOperationException">Thrown when connection string is missing or client cannot connect.</exception>
    private ServiceClient CreateServiceClient()
    {
        var clientId = _configuration["ClientId"];
        var clientSecret = _configuration["ClientSecret"];
        var environment = _configuration["DataverseEnvironment"];

        var connectionString = $"Url=https://{environment};AuthType=ClientSecret;ClientId={clientId};ClientSecret={clientSecret};RequireNewInstance=true";

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(Constants.ErrorMessages.DataverseConnectionStringMissing);
        }

        var client = new ServiceClient(connectionString);

        if (!client.IsReady)
        {
            _logger.LogError(
                Constants.LogMessages.DataverseServiceClientNotReady,
                client.LastError);

            throw new InvalidOperationException(Constants.ErrorMessages.UnableToConnectToDataverse);
        }

        return client;
    }

    /// <summary>
    /// Maps a Dataverse Entity to an EnquiryResponse model.
    /// </summary>
    /// <param name="entity">The Dataverse entity to map.</param>
    /// <returns>An EnquiryResponse populated with data from the entity.</returns>
    private static EnquiryResponse MapEntityToResponse(Entity entity)
    {
        return new EnquiryResponse
        {
            Id = entity.Id,
            Name = entity.GetAttributeValue<string>(Constants.Dataverse.Columns.NewColumn),
            Email = entity.GetAttributeValue<string>(Constants.Dataverse.Columns.Email),
            Message = entity.GetAttributeValue<string>(Constants.Dataverse.Columns.Message),
            Source = entity.GetAttributeValue<string>(Constants.Dataverse.Columns.Source),
            CreatedOn = entity.GetAttributeValue<DateTime?>(Constants.Dataverse.Columns.CreatedOn),
            ModifiedOn = entity.GetAttributeValue<DateTime?>(Constants.Dataverse.Columns.ModifiedOn)
        };
    }

    /// <summary>
    /// Validates that all fields in the create request meet length constraints.
    /// </summary>
    /// <param name="request">The create request to validate.</param>
    /// <exception cref="ArgumentException">Thrown when any field exceeds its maximum allowed length.</exception>
    private void ValidateFieldLengths(CreateEnquiryRequest request)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name.Length > Constants.Dataverse.MaxLength.Name)
        {
            errors.Add(string.Format(Constants.ErrorMessages.NameTooLong, Constants.Dataverse.MaxLength.Name));
        }

        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email.Length > Constants.Dataverse.MaxLength.Email)
        {
            errors.Add(string.Format(Constants.ErrorMessages.EmailTooLong, Constants.Dataverse.MaxLength.Email));
        }

        if (!string.IsNullOrWhiteSpace(request.Message) && request.Message.Length > Constants.Dataverse.MaxLength.Message)
        {
            errors.Add(string.Format(Constants.ErrorMessages.MessageTooLong, Constants.Dataverse.MaxLength.Message));
        }

        if (!string.IsNullOrWhiteSpace(request.Source) && request.Source.Length > Constants.Dataverse.MaxLength.Source)
        {
            errors.Add(string.Format(Constants.ErrorMessages.SourceTooLong, Constants.Dataverse.MaxLength.Source));
        }

        if (errors.Any())
        {
            var errorMessage = string.Join(" ", errors);
            _logger.LogWarning(Constants.LogMessages.ValidationFailed, errorMessage);
            throw new ArgumentException(errorMessage);
        }
    }

    /// <summary>
    /// Validates that all fields in the update request meet length constraints.
    /// </summary>
    /// <param name="request">The update request to validate.</param>
    /// <exception cref="ArgumentException">Thrown when any field exceeds its maximum allowed length.</exception>
    private void ValidateFieldLengthsForUpdate(UpdateEnquiryRequest request)
    {
        var errors = new List<string>();

        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name.Length > Constants.Dataverse.MaxLength.Name)
        {
            errors.Add(string.Format(Constants.ErrorMessages.NameTooLong, Constants.Dataverse.MaxLength.Name));
        }

        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email.Length > Constants.Dataverse.MaxLength.Email)
        {
            errors.Add(string.Format(Constants.ErrorMessages.EmailTooLong, Constants.Dataverse.MaxLength.Email));
        }

        if (!string.IsNullOrWhiteSpace(request.Message) && request.Message.Length > Constants.Dataverse.MaxLength.Message)
        {
            errors.Add(string.Format(Constants.ErrorMessages.MessageTooLong, Constants.Dataverse.MaxLength.Message));
        }

        if (!string.IsNullOrWhiteSpace(request.Source) && request.Source.Length > Constants.Dataverse.MaxLength.Source)
        {
            errors.Add(string.Format(Constants.ErrorMessages.SourceTooLong, Constants.Dataverse.MaxLength.Source));
        }

        if (errors.Any())
        {
            var errorMessage = string.Join(" ", errors);
            _logger.LogWarning(Constants.LogMessages.ValidationFailed, errorMessage);
            throw new ArgumentException(errorMessage);
        }
    }

    /// <summary>
    /// Truncates a string if it exceeds the specified maximum length.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum allowed length.</param>
    /// <returns>The original string if within limit; otherwise, a truncated substring.</returns>
    private static string? TruncateIfNeeded(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        return value.Length > maxLength ? value.Substring(0, maxLength) : value;
    }
}
