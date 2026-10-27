namespace DeepDrive.UiKit.Abstractions;

/// <summary>
/// Interface for paginated search queries with common parameters.
/// </summary>
public interface ISearchQuery
{
    string? Q { get; set; }
    int Page { get; set; }
    int PageSize { get; set; }
}

/// <summary>
/// Interface for search queries that support active/inactive filtering.
/// </summary>
public interface IActiveFilterableQuery : ISearchQuery
{
    bool? ActiveOnly { get; set; }
}
