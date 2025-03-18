using System.Text.Json.Serialization;
using Wpf.Ui.Animations;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Abstractions.Models.Settings;

[Serializable]
public sealed class DecompositionSettings
{
    [JsonPropertyName("IncludePrivate")] public bool IncludePrivate { get; set; }
    [JsonPropertyName("IncludeFields")] public bool IncludeFields { get; set; }
    [JsonPropertyName("IncludeStatic")] public bool IncludeStatic { get; set; }
    [JsonPropertyName("IncludeEvents")] public bool IncludeEvents { get; set; }
    [JsonPropertyName("IncludeExtensions")] public bool IncludeExtensions { get; set; }
    [JsonPropertyName("IncludeUnsupported")] public bool IncludeUnsupported { get; set; }
    [JsonPropertyName("IncludeRoot")] public bool IncludeRoot { get; set; }

    [JsonPropertyName("ShowTimeColumn")] public bool ShowTimeColumn { get; set; }
    [JsonPropertyName("ShowMemoryColumn")] public bool ShowMemoryColumn { get; set; }
}