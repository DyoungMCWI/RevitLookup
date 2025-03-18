namespace RevitLookup.Abstractions.Options;

/// <summary>
///     Defines storage locations for application data that roams with the user across multiple devices.
/// </summary>
/// <remarks>
///     Used for storing user-specific settings, configurations, or small data files  
///     that need to be available on all devices in a domain environment (e.g., Active Directory).  
///     Typical usage includes user preferences, UI settings, or lightweight configuration files.
/// </remarks>
public sealed partial class ResourceLocationsOptions
{
    /// <summary>
    ///     Add-in configurations directory.
    /// </summary>
    public required string SettingsDirectory { get; set; }

    /// <summary>
    ///     Application settings file path.
    /// </summary>
    public required string ApplicationSettingsPath { get; set; }
    
    /// <summary>
    ///     LookupEngine settings file path.
    /// </summary>
    public required string DecompositionSettingsPath { get; set; }
    
    /// <summary>
    ///     Visualization settings file path.
    /// </summary>
    public required string VisualizationSettingsPath { get; set; }
}