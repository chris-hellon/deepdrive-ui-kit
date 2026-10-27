using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;

namespace DeepDrive.UiKit.Demo.Widgets;

// EasyApi handlers for the sample domain. On the server, ICall<TRequest,TResult> dispatches
// straight to these in-process. They back onto the in-memory WidgetStore.

public sealed class SearchWidgetsQueryHandler(WidgetStore store)
    : IHandle<SearchWidgetsQuery, SearchWidgetsResult>
{
    public Task<HttpResult<SearchWidgetsResult>> Handle(SearchWidgetsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<WidgetDto> query = store.All();

        if (!string.IsNullOrWhiteSpace(request.Q))
        {
            var q = request.Q.Trim();
            query = query.Where(w =>
                w.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                w.Category.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        if (request.ActiveOnly == true)
            query = query.Where(w => w.IsActive);

        var all = query.OrderBy(w => w.Name).ToList();
        var page = Math.Max(1, request.Page);
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
        var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var result = new SearchWidgetsResult
        {
            Items = items,
            TotalCount = all.Count,
            Page = page,
            PageSize = pageSize
        };
        return Task.FromResult(HttpResult<SearchWidgetsResult>.Ok(result));
    }
}

public sealed class GetWidgetByIdQueryHandler(WidgetStore store)
    : IHandle<GetWidgetByIdQuery, WidgetDto>
{
    public Task<HttpResult<WidgetDto>> Handle(GetWidgetByIdQuery request, CancellationToken cancellationToken)
    {
        var widget = store.Find(request.Id);
        return Task.FromResult(widget is null
            ? HttpResult<WidgetDto>.NotFound(new WidgetDto())
            : HttpResult<WidgetDto>.Ok(widget));
    }
}

public sealed class CreateWidgetCommandHandler(WidgetStore store)
    : IHandle<CreateWidgetCommand, Guid>
{
    public Task<HttpResult<Guid>> Handle(CreateWidgetCommand request, CancellationToken cancellationToken)
    {
        var id = store.Add(new WidgetDto
        {
            Name = request.Name.Trim(),
            Category = request.Category.Trim(),
            PriceUsd = request.PriceUsd,
            Description = request.Description?.Trim()
        });
        return Task.FromResult(HttpResult<Guid>.Ok(id));
    }
}

public sealed class UpdateWidgetCommandHandler(WidgetStore store)
    : IHandle<UpdateWidgetCommand, bool>
{
    public Task<HttpResult<bool>> Handle(UpdateWidgetCommand request, CancellationToken cancellationToken)
    {
        var ok = store.Update(new WidgetDto
        {
            Id = request.Id,
            Name = request.Name.Trim(),
            Category = request.Category.Trim(),
            PriceUsd = request.PriceUsd,
            Description = request.Description?.Trim()
        });
        return Task.FromResult(ok ? HttpResult<bool>.Ok(true) : HttpResult<bool>.NotFound(false));
    }
}

public sealed class DeleteWidgetCommandHandler(WidgetStore store)
    : IHandle<DeleteWidgetCommand, bool>
{
    public Task<HttpResult<bool>> Handle(DeleteWidgetCommand request, CancellationToken cancellationToken)
    {
        var ok = store.Remove(request.Id);
        return Task.FromResult(ok ? HttpResult<bool>.Ok(true) : HttpResult<bool>.NotFound(false));
    }
}

public sealed class ToggleWidgetActiveCommandHandler(WidgetStore store)
    : IHandle<ToggleWidgetActiveCommand, bool>
{
    public Task<HttpResult<bool>> Handle(ToggleWidgetActiveCommand request, CancellationToken cancellationToken)
    {
        var ok = store.SetActive(request.Id, request.IsActive);
        return Task.FromResult(ok ? HttpResult<bool>.Ok(true) : HttpResult<bool>.NotFound(false));
    }
}
