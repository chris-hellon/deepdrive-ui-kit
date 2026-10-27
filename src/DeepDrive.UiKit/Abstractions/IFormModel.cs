namespace DeepDrive.UiKit.Abstractions;

/// <summary>
/// Interface for form models used in admin Create/Edit pages.
/// Provides consistent mapping between form data and API commands.
/// </summary>
/// <typeparam name="TCreateCommand">The command type for creating new entities</typeparam>
/// <typeparam name="TUpdateCommand">The command type for updating existing entities</typeparam>
public interface IFormModel<out TCreateCommand, out TUpdateCommand>
{
    /// <summary>
    /// Maps the form model to a create command.
    /// </summary>
    TCreateCommand ToCreateCommand();

    /// <summary>
    /// Maps the form model to an update command.
    /// </summary>
    TUpdateCommand ToUpdateCommand();
}
