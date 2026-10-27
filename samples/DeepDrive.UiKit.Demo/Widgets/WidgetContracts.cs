using BlazorUtils.EasyApi;
using DeepDrive.UiKit.Abstractions;

namespace DeepDrive.UiKit.Demo.Widgets;

// A small sample domain ("Widgets") that exercises the DeepDrive UI Kit admin framework.
// The DTO + commands implement the kit's abstraction interfaces so GenericAdminListPage /
// GenericAdminCreatePage / GenericAdminEditPage can drive them with zero custom plumbing.

public sealed class WidgetDto : IActivatableNamedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal PriceUsd { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public sealed class SearchWidgetsResult : ISearchResult<WidgetDto>
{
    public IReadOnlyList<WidgetDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}

[Route("api/widgets/search")]
public sealed class SearchWidgetsQuery : IGet<SearchWidgetsResult>, IActiveFilterableQuery
{
    [QueryStringParam] public string? Q { get; set; }
    [QueryStringParam] public int Page { get; set; } = 1;
    [QueryStringParam] public int PageSize { get; set; } = 10;
    [QueryStringParam] public bool? ActiveOnly { get; set; }
}

[Route("api/widgets/get")]
public sealed class GetWidgetByIdQuery : IGet<WidgetDto>
{
    [QueryStringParam] public Guid Id { get; set; }
}

[Route("api/widgets/create")]
public sealed class CreateWidgetCommand : IPost<Guid>
{
    [BodyParam] public string Name { get; set; } = string.Empty;
    [BodyParam] public string Category { get; set; } = string.Empty;
    [BodyParam] public decimal PriceUsd { get; set; }
    [BodyParam] public string? Description { get; set; }
}

[Route("api/widgets/update")]
public sealed class UpdateWidgetCommand : IPost<bool>
{
    [BodyParam] public Guid Id { get; set; }
    [BodyParam] public string Name { get; set; } = string.Empty;
    [BodyParam] public string Category { get; set; } = string.Empty;
    [BodyParam] public decimal PriceUsd { get; set; }
    [BodyParam] public string? Description { get; set; }
}

[Route("api/widgets/delete")]
public sealed class DeleteWidgetCommand : IPost<bool>, IDeleteByIdCommand
{
    [BodyParam] public Guid Id { get; set; }
}

[Route("api/widgets/toggle-active")]
public sealed class ToggleWidgetActiveCommand : IPost<bool>, IToggleActiveCommand
{
    [BodyParam] public Guid Id { get; set; }
    [BodyParam] public bool IsActive { get; set; }
}
