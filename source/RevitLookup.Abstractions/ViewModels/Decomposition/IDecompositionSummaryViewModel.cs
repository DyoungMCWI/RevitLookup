using System.Collections.ObjectModel;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.ViewModels.Decomposition;

/// <summary>
///     Represents the data for the Decomposition Summary view.
/// </summary>
public interface IDecompositionSummaryViewModel : ISummaryViewModel
{
    /// <summary>
    ///     The list of filtered decomposed objects.
    /// </summary>
    ObservableCollection<ObservableDecomposedObjectsGroup> FilteredDecomposedObjects { get; }
    
    /// <summary>
    ///     Remove an item from the decomposed objects.
    /// </summary>
    void RemoveItem(object target);
}