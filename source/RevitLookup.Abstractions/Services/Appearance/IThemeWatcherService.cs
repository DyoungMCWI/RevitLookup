using System.Windows;

namespace RevitLookup.Abstractions.Services.Appearance;

/// <summary>
///     Service that provide methods for applying themes to the components and watching for theme changes.
/// </summary>
public interface IThemeWatcherService
{
    /// <summary>
    ///     Initialize the UI components and resources.
    /// </summary>
    void Initialize();
    
    /// <summary>
    ///     Apply the current theme to the whole application and monitor for changes.
    /// </summary>
    void ApplyTheme();
    
    /// <summary>
    ///     Watch for theme changes on the specified <see cref="FrameworkElement"/>.
    /// </summary>
    void Watch(FrameworkElement frameworkElement);
    
    /// <summary>
    ///     Stop watching for theme changes.
    /// </summary>
    void Unwatch();
}