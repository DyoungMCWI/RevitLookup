using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.Services.Decomposition;

/// <summary>
///     Service for searching decomposition objects and members
/// </summary>
public interface IDecompositionSearchService
{
    /// <summary>
    ///     Search for objects and members by query.
    /// </summary>
    (List<ObservableDecomposedObject> FilteredObjects, List<ObservableDecomposedMember> FilteredMembers) Search(
        string query,
        ObservableDecomposedObject? selectedObject,
        List<ObservableDecomposedObject> objects);

    /// <summary>
    ///     Search for members by query.
    /// </summary>
    List<ObservableDecomposedMember> SearchMembers(string query, ObservableDecomposedObject value);
}