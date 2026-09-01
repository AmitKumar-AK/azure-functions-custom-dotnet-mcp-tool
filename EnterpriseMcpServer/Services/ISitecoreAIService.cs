using EnterpriseMcpServer.Models;

namespace EnterpriseMcpServer.Services;

/// <summary>
/// Service interface for fetching Sitecore item details via the Edge GraphQL API.
/// </summary>
public interface ISitecoreAIService
{
    /// <summary>
    /// Fetches a Sitecore item and its children by path and language.
    /// </summary>
    /// <param name="path">The Sitecore item path or item ID.</param>
    /// <param name="language">The language version to retrieve (e.g. "en").</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The item response if found; otherwise, null.</returns>
    Task<SitecoreItemResponse?> GetItemChildDetailsAsync(string path, string language, CancellationToken cancellationToken = default);
}
