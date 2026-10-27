namespace DeepDrive.UiKit.Toasts;

public static class ToastBroker
{
    public record ToastMessage(Guid Id, string Message, bool Success, DateTime ExpiresAt, string? Title = null, string? Url = null, Guid? NotificationId = null);

    public static event Action<ToastMessage>? OnToast;

    public static void Success(string message, int durationMs = 2500)
        => Show(message, true, durationMs);

    public static void Error(string message, int durationMs = 2500)
        => Show(message, false, durationMs);

    public static void Show(string message, bool success, int durationMs = 2500, string? title = null, string? url = null)
    {
        var id = Guid.NewGuid();
        var toast = new ToastMessage(id, message, success, DateTime.UtcNow.AddMilliseconds(durationMs), title, url);
        OnToast?.Invoke(toast);
    }

    /// <summary>
    /// Shows a notification toast with URL for navigation.
    /// </summary>
    public static void Notification(string message, string? url = null, int durationMs = 4000, Guid? notificationId = null)
    {
        var id = Guid.NewGuid();
        var toast = new ToastMessage(id, message, true, DateTime.UtcNow.AddMilliseconds(durationMs), "Notification", url, notificationId);
        OnToast?.Invoke(toast);
    }
}
