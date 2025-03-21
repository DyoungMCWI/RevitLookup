using RevitLookup.Abstractions.Models.AboutProgram;

namespace RevitLookup.Abstractions.ViewModels.AboutProgram;

/// <summary>
///     Represents the data for the OpenSource view.
/// </summary>
public interface IOpenSourceViewModel
{
    /// <summary>
    ///     The list of open-source software used in the application.
    /// </summary>
    List<OpenSourceSoftware> Software { get; }
}