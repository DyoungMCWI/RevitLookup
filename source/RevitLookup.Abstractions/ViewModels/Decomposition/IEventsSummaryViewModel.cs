using System.Collections.ObjectModel;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.Abstractions.ViewModels.Decomposition;

/// <summary>
///     Represents the data for the Events Summary view.
/// </summary>
public interface IEventsSummaryViewModel : ISummaryViewModel, INavigationAware
{
    /// <summary>
    ///     The list of filtered decomposed objects.
    /// </summary>
    ObservableCollection<ObservableDecomposedObject> FilteredDecomposedObjects { get; }
}