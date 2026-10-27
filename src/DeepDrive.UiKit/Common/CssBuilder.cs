namespace DeepDrive.UiKit.Common;

public sealed class CssBuilder
{
    private readonly List<string> _parts = [];
    public CssBuilder Add(string? cls) { if (!string.IsNullOrWhiteSpace(cls)) _parts.Add(cls); return this; }
    public CssBuilder AddIf(Func<string> clsFactory, bool condition)
    {
        if (condition) _parts.Add(clsFactory());
        return this;
    }
    public override string ToString() => string.Join(" ", _parts);
}