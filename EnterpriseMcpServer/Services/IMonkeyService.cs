using EnterpriseMcpServer.Models;

namespace EnterpriseMcpServer.Services;

/// <summary>
/// Service interface for retrieving monkey records from the remote JSON feed.
/// </summary>
public interface IMonkeyService
{
    /// <summary>
    /// Retrieves all monkey records from the remote feed.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A collection of monkey responses.</returns>
    Task<IEnumerable<MonkeyResponse>> GetMonkeysAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single monkey record by name.
    /// </summary>
    /// <param name="name">The name of the monkey to find.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The monkey response if found; otherwise, null.</returns>
    Task<MonkeyResponse?> GetMonkeyByNameAsync(string name, CancellationToken cancellationToken);
}
