using RevitLookup.Abstractions.Models.Settings;

namespace RevitLookup.Abstractions.Services.Settings;

public interface ISettingsService
{
    public ApplicationSettings ApplicationSettings { get; }
    public DecompositionSettings DecompositionSettings { get; }
    public VisualizationSettings VisualizationSettings { get; }
    void SaveSettings();
    void LoadSettings();
    void ResetApplicationSettings();
    void ResetDecompositionSettings();
    void ResetVisualizationSettings();
}