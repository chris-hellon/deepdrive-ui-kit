namespace DeepDrive.UiKit.Abstractions;

/// <summary>
/// Context passed to Footer RenderFragment with model, saving state, and save action.
/// Used by GenericAdminCreatePage and GenericAdminEditPage.
/// </summary>
public record FormContext<TModel>(TModel Model, bool IsSaving, Func<Task> SaveAsync);
