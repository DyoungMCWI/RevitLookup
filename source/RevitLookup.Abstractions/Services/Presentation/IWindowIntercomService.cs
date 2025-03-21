using System.Windows;
using System.Windows.Threading;

namespace RevitLookup.Abstractions.Services.Presentation;

/// <summary>
///     Manage the RevitLookup instances lifecycle.
/// </summary>
public interface IWindowIntercomService
{
    /// <summary>
    ///     Get the dispatcher for the UI thread.
    /// </summary>
    Dispatcher Dispatcher { get; }
    
    /// <summary>
    ///     Get all opened instances.
    /// </summary>
    List<Window> OpenedWindows { get; }

    /// <summary>
    ///     Get the current Window host.
    /// </summary>
    Window GetHost();
    
    /// <summary>
    ///     Set the private Window host. It won't be available in the <see cref="OpenedWindows"/> property.
    /// </summary>
    void SetHost(Window host);
    
    /// <summary>
    ///     Set the shared Window host. It will be available in the <see cref="OpenedWindows"/> property.
    /// </summary>
    void SetSharedHost(Window host);
}