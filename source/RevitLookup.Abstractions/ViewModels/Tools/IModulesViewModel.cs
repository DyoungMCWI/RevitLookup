using RevitLookup.Abstractions.Models.Tools;

namespace RevitLookup.Abstractions.ViewModels.Tools;

/// <summary>
///     Represents the data for the Modules view.
/// </summary>
public interface IModulesViewModel
{
    /// <summary>
    ///     The search query for filtering modules.
    /// </summary>
    string SearchText { get; set; }

    /// <summary>
    ///     The list of filtered modules.
    /// </summary>
    List<ModuleInfo> FilteredModules { get; set; }

    /// <summary>
    ///     The list of all assembly modules.
    /// </summary>
    List<ModuleInfo> Modules { get; set; }
}