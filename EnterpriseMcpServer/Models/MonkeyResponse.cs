namespace EnterpriseMcpServer.Models;

/// <summary>
/// Response model representing a monkey record retrieved from the remote JSON feed.
/// </summary>
public sealed class MonkeyResponse
{
    /// <summary>Gets or sets the name of the monkey.</summary>
    public string? Name { get; set; }

    /// <summary>Gets or sets the geographic location of the monkey.</summary>
    public string? Location { get; set; }

    /// <summary>Gets or sets descriptive details about the monkey.</summary>
    public string? Details { get; set; }

    /// <summary>Gets or sets the URL of the monkey's image.</summary>
    public string? Image { get; set; }

    /// <summary>Gets or sets the estimated population count.</summary>
    public int Population { get; set; }

    /// <summary>Gets or sets the latitude coordinate of the monkey's habitat.</summary>
    public double Latitude { get; set; }

    /// <summary>Gets or sets the longitude coordinate of the monkey's habitat.</summary>
    public double Longitude { get; set; }
}
