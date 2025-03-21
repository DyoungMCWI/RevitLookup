namespace RevitLookup.Abstractions.ViewModels.Tools;

/// <summary>
///     Represents the data for the Search Elements view.
/// </summary>
public interface ISearchElementsViewModel
{
    /// <summary>
    ///     The search query for filtering elements.
    /// </summary>
    string SearchText { get; set; }

    /// <summary>
    ///     Search for elements in the current document and visualize them.
    /// </summary>
    Task<bool> SearchElementsAsync();
}