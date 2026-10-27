namespace DeepDrive.UiKit.Abstractions;

/// <summary>
/// Interface for entities that have an Id and Name, used by admin list pages.
/// </summary>
public interface INamedEntity
{
    Guid Id { get; }
    string Name { get; }
}

/// <summary>
/// Interface for entities that can be activated/deactivated.
/// </summary>
public interface IActivatable
{
    bool IsActive { get; }
}

/// <summary>
/// Combined interface for named entities that can be activated/deactivated.
/// </summary>
public interface IActivatableNamedEntity : INamedEntity, IActivatable
{
}
