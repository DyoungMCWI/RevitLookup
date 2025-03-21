using RevitLookup.Abstractions.Models.Tools;

namespace RevitLookup.Abstractions.ViewModels.Tools;

/// <summary>
///     Represents the data for the Units view.
/// </summary>
public interface IUnitsViewModel
{
    /// <summary>
    ///     The list of all units.
    /// </summary>
    List<UnitInfo> Units { get; set; }

    /// <summary>
    ///     The list of filtered units.
    /// </summary>
    List<UnitInfo> FilteredUnits { get; set; }

    /// <summary>
    ///     The search query for filtering units.
    /// </summary>
    string SearchText { get; set; }

    /// <summary>
    ///     Initialize parameters for representation.
    /// </summary>
    void InitializeParameters();

    /// <summary>
    ///     Initialize categories for representation.
    /// </summary>
    void InitializeCategories();

    /// <summary>
    ///     Initialize Forge schema for representation.
    /// </summary>
    void InitializeForgeSchema();

    /// <summary>
    ///     Decompose unit information and visualize it.
    /// </summary>
    Task DecomposeAsync(UnitInfo unitInfo);
}