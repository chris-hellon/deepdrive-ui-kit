using Microsoft.AspNetCore.Components;

namespace DeepDrive.UiKit.Admin;

/// <summary>
/// Base class for admin list pages with common pagination, search, and confirm dialog state.
/// </summary>
/// <typeparam name="TItem">The DTO type for list items.</typeparam>
/// <typeparam name="TResult">The search result type.</typeparam>
public abstract class AdminListPageBase<TItem, TResult> : ComponentBase
    where TItem : class
    where TResult : class
{
    // Common state
    protected bool _loading = true;
    protected string? _error;
    protected TResult? _result;
    protected string _search = string.Empty;
    protected bool _filterVisible;
    protected int _page = 1;

    /// <summary>
    /// Override to specify the page size. Default is 15.
    /// </summary>
    protected virtual int PageSize => 15;

    // Confirm dialog state
    protected bool _showDeleteConfirm;
    protected bool _showDeactivateConfirm;
    protected bool _showActivateConfirm;
    protected bool _confirmBusy;
    protected string? _confirmError;
    protected TItem? _selectedItem;

    /// <summary>
    /// Override to determine if there are any active filters beyond search.
    /// </summary>
    protected virtual bool HasActiveFilters => !string.IsNullOrEmpty(_search);

    protected override async Task OnInitializedAsync()
    {
        await RefreshAsync();
    }

    /// <summary>
    /// Override to implement the search/refresh logic.
    /// </summary>
    protected abstract Task RefreshAsync();

    /// <summary>
    /// Called when search value changes.
    /// </summary>
    protected virtual async Task OnSearchChanged()
    {
        _page = 1;
        await RefreshAsync();
    }

    /// <summary>
    /// Override to clear page-specific filters.
    /// </summary>
    protected virtual async Task ClearFilters()
    {
        _search = string.Empty;
        _page = 1;
        await RefreshAsync();
    }

    /// <summary>
    /// Navigate to previous page.
    /// </summary>
    protected async Task OnPreviousPage()
    {
        if (_page > 1)
        {
            _page--;
            await RefreshAsync();
        }
    }

    /// <summary>
    /// Navigate to next page.
    /// </summary>
    protected async Task OnNextPage()
    {
        if (_result is not null && _page < GetTotalPages())
        {
            _page++;
            await RefreshAsync();
        }
    }

    /// <summary>
    /// Override to return the total pages from the result.
    /// </summary>
    protected abstract int GetTotalPages();

    /// <summary>
    /// Show delete confirmation dialog for the specified item.
    /// </summary>
    protected void PromptDelete(TItem item)
    {
        _selectedItem = item;
        _confirmError = null;
        _showDeleteConfirm = true;
    }

    /// <summary>
    /// Show deactivate confirmation dialog for the specified item.
    /// </summary>
    protected void PromptDeactivate(TItem item)
    {
        _selectedItem = item;
        _confirmError = null;
        _showDeactivateConfirm = true;
    }

    /// <summary>
    /// Show activate confirmation dialog for the specified item.
    /// </summary>
    protected void PromptActivate(TItem item)
    {
        _selectedItem = item;
        _confirmError = null;
        _showActivateConfirm = true;
    }

    /// <summary>
    /// Override to implement delete logic. Call base for cleanup.
    /// </summary>
    protected virtual async Task OnConfirmDelete()
    {
        if (_selectedItem is null) return;
        _confirmBusy = true;
        _confirmError = null;

        try
        {
            await ExecuteDeleteAsync(_selectedItem);
            _showDeleteConfirm = false;
            _selectedItem = null;
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _confirmError = GetDeleteErrorMessage(ex);
        }
        finally
        {
            _confirmBusy = false;
        }
    }

    /// <summary>
    /// Override to implement deactivate logic. Call base for cleanup.
    /// </summary>
    protected virtual async Task OnConfirmDeactivate()
    {
        if (_selectedItem is null) return;
        _confirmBusy = true;
        _confirmError = null;

        try
        {
            await ExecuteDeactivateAsync(_selectedItem);
            _showDeactivateConfirm = false;
            _selectedItem = null;
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _confirmError = GetDeactivateErrorMessage(ex);
        }
        finally
        {
            _confirmBusy = false;
        }
    }

    /// <summary>
    /// Override to implement activate logic. Call base for cleanup.
    /// </summary>
    protected virtual async Task OnConfirmActivate()
    {
        if (_selectedItem is null) return;
        _confirmBusy = true;
        _confirmError = null;

        try
        {
            await ExecuteActivateAsync(_selectedItem);
            _showActivateConfirm = false;
            _selectedItem = null;
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _confirmError = GetActivateErrorMessage(ex);
        }
        finally
        {
            _confirmBusy = false;
        }
    }

    /// <summary>
    /// Override to implement the actual delete API call.
    /// </summary>
    protected virtual Task ExecuteDeleteAsync(TItem item) => Task.CompletedTask;

    /// <summary>
    /// Override to implement the actual deactivate API call.
    /// </summary>
    protected virtual Task ExecuteDeactivateAsync(TItem item) => Task.CompletedTask;

    /// <summary>
    /// Override to implement the actual activate API call.
    /// </summary>
    protected virtual Task ExecuteActivateAsync(TItem item) => Task.CompletedTask;

    /// <summary>
    /// Override to customize the delete error message.
    /// </summary>
    protected virtual string GetDeleteErrorMessage(Exception ex) => $"Failed to delete {EntityName.ToLower()}.";

    /// <summary>
    /// Override to customize the deactivate error message.
    /// </summary>
    protected virtual string GetDeactivateErrorMessage(Exception ex) => $"Failed to deactivate {EntityName.ToLower()}.";

    /// <summary>
    /// Override to customize the activate error message.
    /// </summary>
    protected virtual string GetActivateErrorMessage(Exception ex) => $"Failed to activate {EntityName.ToLower()}.";

    /// <summary>
    /// Override to specify the entity name (e.g., "Tour", "Course"). Used in dialog messages.
    /// </summary>
    protected virtual string EntityName => "Item";

    /// <summary>
    /// Override to get the name of the selected item for display in dialogs.
    /// </summary>
    protected virtual string? GetSelectedItemName() => null;

    /// <summary>
    /// Override to specify the success message after delete.
    /// </summary>
    protected virtual string DeleteSuccessMessage => $"{EntityName} deleted.";

    /// <summary>
    /// Override to specify the success message after deactivate.
    /// </summary>
    protected virtual string DeactivateSuccessMessage => $"{EntityName} deactivated.";

    /// <summary>
    /// Override to specify the success message after activate.
    /// </summary>
    protected virtual string ActivateSuccessMessage => $"{EntityName} activated.";

    // Confirm dialog message helpers
    protected string DeleteDialogTitle => $"Delete {EntityName}";
    protected string DeleteDialogMessage => $"Are you sure you want to permanently delete '{GetSelectedItemName()}'? This action cannot be undone.";

    protected string DeactivateDialogTitle => $"Deactivate {EntityName}";
    protected string DeactivateDialogMessage => $"Are you sure you want to deactivate '{GetSelectedItemName()}'? It will no longer be available for new bookings.";

    protected string ActivateDialogTitle => $"Activate {EntityName}";
    protected string ActivateDialogMessage => $"Are you sure you want to activate '{GetSelectedItemName()}'? It will become available for bookings again.";
}
