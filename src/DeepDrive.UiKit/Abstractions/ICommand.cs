namespace DeepDrive.UiKit.Abstractions;

/// <summary>
/// Interface for delete commands that operate on an entity by ID.
/// </summary>
public interface IDeleteByIdCommand
{
    Guid Id { get; set; }
}

/// <summary>
/// Interface for toggle active commands.
/// </summary>
public interface IToggleActiveCommand
{
    Guid Id { get; set; }
    bool IsActive { get; set; }
}
