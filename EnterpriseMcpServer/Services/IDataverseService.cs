
using EnterpriseMcpServer.Models;

namespace EnterpriseMcpServer.Services;

/// <summary>
/// Service interface for managing enquiry records in Microsoft Dataverse.
/// Provides CRUD operations and query capabilities for enquiry data.
/// </summary>
public interface IDataverseService
{

    /// <summary>
    /// Creates a new enquiry record in Dataverse.
    /// </summary>
    /// <param name="request">The request containing enquiry details to create.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The unique identifier (GUID) of the newly created enquiry.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails or field lengths exceed maximum allowed values.</exception>
    Task<Guid> CreateEnquiryAsync(CreateEnquiryRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple enquiries from Dataverse, ordered by creation date descending.
    /// </summary>
    /// <param name="topCount">The maximum number of enquiries to retrieve. Default is 100.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A collection of enquiry responses.</returns>
    Task<IEnumerable<EnquiryResponse>> GetEnquiriesAsync(int topCount = 100, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single enquiry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the enquiry.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The enquiry response if found; otherwise, null.</returns>
    Task<EnquiryResponse?> GetEnquiryByIdAsync(Guid id, CancellationToken cancellationToken);
}