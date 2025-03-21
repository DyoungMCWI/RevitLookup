namespace RevitLookup.Abstractions.Services.Presentation;

/// <summary>
///     Service for displaying notifications to the user.
/// </summary>
public interface INotificationService
{
    /// <summary>
    ///     Show a success notification to the user.
    /// </summary>
    void ShowSuccess(string title, string message);
    
    /// <summary>
    ///     Show a warning notification to the user.
    /// </summary>
    void ShowWarning(string title, string message);
    
    /// <summary>
    ///     Show an error notification to the user.
    /// </summary>
    void ShowError(string title, string message);
    
    /// <summary>
    ///     Show an error notification to the user.
    /// </summary>
    void ShowError(string title, Exception exception);
}