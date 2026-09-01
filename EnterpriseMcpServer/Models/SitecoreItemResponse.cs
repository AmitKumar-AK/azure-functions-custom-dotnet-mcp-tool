namespace EnterpriseMcpServer.Models;

/// <summary>
/// Response model representing a Sitecore item and its children retrieved via Edge GraphQL.
/// </summary>
public sealed class SitecoreItemResponse
{
    /// <summary>Gets or sets the unique identifier of the item.</summary>
    public string? Id { get; set; }

    /// <summary>Gets or sets the item name.</summary>
    public string? Name { get; set; }

    /// <summary>Gets or sets the full Sitecore path of the item.</summary>
    public string? Path { get; set; }

    /// <summary>Gets or sets the display name of the item.</summary>
    public string? DisplayName { get; set; }

    /// <summary>Gets or sets whether the item has child items.</summary>
    public bool HasChildren { get; set; }

    /// <summary>Gets or sets the children of the item.</summary>
    public SitecoreItemChildren? Children { get; set; }
}

/// <summary>
/// Wrapper for the children collection returned by the Edge GraphQL query.
/// </summary>
public sealed class SitecoreItemChildren
{
    /// <summary>Gets or sets the list of child item summaries.</summary>
    public List<SitecoreItemChild>? Results { get; set; }
}

/// <summary>
/// Summary model for a child Sitecore item.
/// </summary>
public sealed class SitecoreItemChild
{
    /// <summary>Gets or sets the unique identifier of the child item.</summary>
    public string? Id { get; set; }

    /// <summary>Gets or sets the name of the child item.</summary>
    public string? Name { get; set; }

    /// <summary>Gets or sets the full Sitecore path of the child item.</summary>
    public string? Path { get; set; }
}
