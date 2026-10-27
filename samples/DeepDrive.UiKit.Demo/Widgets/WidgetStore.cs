namespace DeepDrive.UiKit.Demo.Widgets;

/// <summary>
/// Dead-simple in-memory store so the demo needs no database. Registered as a singleton.
/// </summary>
public sealed class WidgetStore
{
    private readonly List<WidgetDto> _widgets =
    [
        new() { Id = Guid.NewGuid(), Name = "Titanium Driver", Category = "Clubs", PriceUsd = 549, Description = "Low-spin, high-forgiveness driver head.", IsActive = true },
        new() { Id = Guid.NewGuid(), Name = "Tour Blade Irons", Category = "Clubs", PriceUsd = 1299, Description = "Forged players' irons, 4–PW.", IsActive = true },
        new() { Id = Guid.NewGuid(), Name = "Carbon Cart Bag", Category = "Bags", PriceUsd = 329, Description = "14-way top, waterproof.", IsActive = true },
        new() { Id = Guid.NewGuid(), Name = "Rangefinder Pro", Category = "Tech", PriceUsd = 399, Description = "Slope-enabled laser rangefinder.", IsActive = true },
        new() { Id = Guid.NewGuid(), Name = "Spiked Tour Shoes", Category = "Apparel", PriceUsd = 189, Description = "Replaceable-spike tour shoe.", IsActive = false },
        new() { Id = Guid.NewGuid(), Name = "Soft Feel Balls (dz)", Category = "Balls", PriceUsd = 42, Description = "Low-compression distance ball.", IsActive = true },
    ];

    private readonly Lock _gate = new();

    public IReadOnlyList<WidgetDto> All()
    {
        lock (_gate) return _widgets.Select(Clone).ToList();
    }

    public WidgetDto? Find(Guid id)
    {
        lock (_gate)
        {
            var w = _widgets.FirstOrDefault(x => x.Id == id);
            return w is null ? null : Clone(w);
        }
    }

    public Guid Add(WidgetDto widget)
    {
        lock (_gate)
        {
            widget.Id = Guid.NewGuid();
            _widgets.Add(Clone(widget));
            return widget.Id;
        }
    }

    public bool Update(WidgetDto widget)
    {
        lock (_gate)
        {
            var existing = _widgets.FirstOrDefault(x => x.Id == widget.Id);
            if (existing is null) return false;
            existing.Name = widget.Name;
            existing.Category = widget.Category;
            existing.PriceUsd = widget.PriceUsd;
            existing.Description = widget.Description;
            return true;
        }
    }

    public bool SetActive(Guid id, bool isActive)
    {
        lock (_gate)
        {
            var existing = _widgets.FirstOrDefault(x => x.Id == id);
            if (existing is null) return false;
            existing.IsActive = isActive;
            return true;
        }
    }

    public bool Remove(Guid id)
    {
        lock (_gate) return _widgets.RemoveAll(x => x.Id == id) > 0;
    }

    private static WidgetDto Clone(WidgetDto w) => new()
    {
        Id = w.Id, Name = w.Name, Category = w.Category,
        PriceUsd = w.PriceUsd, Description = w.Description, IsActive = w.IsActive
    };
}
