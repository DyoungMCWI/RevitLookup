using System.Text.Json.Serialization;
using Wpf.Ui.Animations;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Abstractions.Models.Settings;

/// <summary>
///     Schema for application settings.
/// </summary>
[Serializable]
public sealed class ApplicationSettings
{
    [JsonPropertyName("Theme")] public ApplicationTheme Theme { get; set; }
    [JsonPropertyName("Background")] public WindowBackdropType Background { get; set; }
    [JsonPropertyName("Transition")] public Transition Transition { get; set; }
    [JsonPropertyName("WindowWidth")] public double WindowWidth { get; set; }
    [JsonPropertyName("WindowHeight")] public double WindowHeight { get; set; }
    [JsonPropertyName("UseHardwareRendering")] public bool UseHardwareRendering { get; set; }
    [JsonPropertyName("UseSizeRestoring")] public bool UseSizeRestoring { get; set; }
    [JsonPropertyName("UseModifyTab")] public bool UseModifyTab { get; set; }
}