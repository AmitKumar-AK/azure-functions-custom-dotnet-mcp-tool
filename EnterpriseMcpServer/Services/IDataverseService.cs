
using EnterpriseMcpServer.Models;

namespace EnterpriseMcpServer.Services;

/// <summary>
/// Service interface for managing enquiry records in Microsoft Dataverse.
/// Provides CRUD operations and query capabilities for enquiry data.
/// </summary>
public interface IDataverseService
{

    /// <summary>
    /// Retrieves multiple enquiries from Dataverse, ordered by creation date descending.
    /// </summary>
    /// <param name="topCount">The maximum number of enquiries to retrieve. Default is 100.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A collection of enquiry responses.</returns>
    Task<IEnumerable<EnquiryResponse>> GetEnquiriesAsync(int topCount = 100, CancellationToken cancellationToken = default);
}