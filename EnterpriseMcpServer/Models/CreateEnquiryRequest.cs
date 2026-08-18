
namespace EnterpriseMcpServer.Models;

/// <summary>
/// Response model representing an enquiry retrieved from Dataverse.
/// </summary>
public sealed class EnquiryResponse
{
    /// <summary>Gets or sets the unique identifier of the enquiry.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the name of the person submitting the enquiry.</summary>
    public string? Name { get; set; }

    /// <summary>Gets or sets the email address of the person submitting the enquiry.</summary>
    public string? Email { get; set; }

    /// <summary>Gets or sets the message content of the enquiry.</summary>
    public string? Message { get; set; }

    /// <summary>Gets or sets the source system from which the enquiry originated.</summary>
    public string? Source { get; set; }

    /// <summary>Gets or sets the date and time when the enquiry was created.</summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>Gets or sets the date and time when the enquiry was last modified.</summary>
    public DateTime? ModifiedOn { get; set; }
}

/// <summary>
/// Request model for creating a new enquiry in Dataverse.
/// </summary>
public sealed class CreateEnquiryRequest
{
    /// <summary>Gets or sets the name of the person submitting the enquiry.</summary>
    public string? Name { get; set; }

    /// <summary>Gets or sets the email address of the person submitting the enquiry.</summary>
    public string? Email { get; set; }

    /// <summary>Gets or sets the message content of the enquiry.</summary>
    public string? Message { get; set; }

    /// <summary>Gets or sets the source system from which the enquiry originated.</summary>
    public string? Source { get; set; }
}

/// <summary>
/// Request model for updating an existing enquiry in Dataverse.
/// </summary>
public sealed class UpdateEnquiryRequest
{
    /// <summary>Gets or sets the unique identifier of the enquiry to update.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the updated name. If null, the name will not be modified.</summary>
    public string? Name { get; set; }

    /// <summary>Gets or sets the updated email address. If null, the email will not be modified.</summary>
    public string? Email { get; set; }

    /// <summary>Gets or sets the updated message content. If null, the message will not be modified.</summary>
    public string? Message { get; set; }

    /// <summary>Gets or sets the updated source. If null, the source will not be modified.</summary>
    public string? Source { get; set; }
}

/// <summary>
/// Request model for deleting an enquiry from Dataverse.
/// </summary>
public sealed class DeleteEnquiryRequest
{
    /// <summary>Gets or sets the unique identifier of the enquiry to delete.</summary>
    public Guid Id { get; set; }
}

/// <summary>
/// Generic API response wrapper for returning data and status information to clients.
/// </summary>
/// <typeparam name="T">The type of data being returned in the response.</typeparam>
public sealed class ApiResponse<T>
{
    /// <summary>Gets or sets a value indicating whether the operation was successful.</summary>
    public bool Success { get; set; }

    /// <summary>Gets or sets a message describing the result of the operation.</summary>
    public string? Message { get; set; }

    /// <summary>Gets or sets the data payload of the response.</summary>
    public T? Data { get; set; }

    /// <summary>
    /// Creates a successful API response with the specified data and optional message.
    /// </summary>
    /// <param name="data">The data to return in the response.</param>
    /// <param name="message">An optional success message.</param>
    /// <returns>A successful API response.</returns>
    public static ApiResponse<T> Ok(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Creates a failed API response with the specified error message.
    /// </summary>
    /// <param name="message">The error message describing why the operation failed.</param>
    /// <returns>A failed API response.</returns>
    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message
        };
    }
}