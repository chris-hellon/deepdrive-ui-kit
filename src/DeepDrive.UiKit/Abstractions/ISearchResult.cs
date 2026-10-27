namespace DeepDrive.UiKit.Abstractions;

/// <summary>
/// Interface for paginated search results.
/// </summary>
/// <typeparam name="TItem">The type of items in the result.</typeparam>
public interface ISearchResult<TItem>
{
    IReadOnlyList<TItem> Items { get; }
    int TotalCount { get; }
    int Page { get; }
    int PageSize { get; }
    int TotalPages { get; }
}
