using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.Abstractions.ObservableModels.Entries;

namespace RevitLookup.Abstractions.ViewModels.Tools;

/// <summary>
///     Represents the data for the Revit Settings view.
/// </summary>
public interface IRevitSettingsViewModel
{
    /// <summary>
    ///     Whether the entries are filtered.
    /// </summary>
    bool Filtered { get; set; }

    /// <summary>
    ///     The category filter for entries.
    /// </summary>
    string CategoryFilter { get; set; }

    /// <summary>
    ///     The property filter for entries.
    /// </summary>
    string PropertyFilter { get; set; }

    /// <summary>
    ///     The value filter for entries.
    /// </summary>
    string ValueFilter { get; set; }

    /// <summary>
    ///     Whether to show user settings filter.
    /// </summary>
    bool ShowUserSettingsFilter { get; set; }

    /// <summary>
    ///     The selected settings entry.
    /// </summary>
    ObservableIniEntry? SelectedEntry { get; set; }

    /// <summary>
    ///     The list of all settings entries.
    /// </summary>
    List<ObservableIniEntry> Entries { get; set; }

    /// <summary>
    ///     The list of filtered settings entries.
    /// </summary>
    ObservableCollection<ObservableIniEntry> FilteredEntries { get; set; }

    /// <summary>
    ///     Show help page.
    /// </summary>
    IRelayCommand ShowHelpCommand { get; }

    /// <summary>
    ///     Open settings popup.
    /// </summary>
    IRelayCommand OpenSettingsCommand { get; }

    /// <summary>
    ///     Clear all filters.
    /// </summary>
    IRelayCommand ClearFiltersCommand { get; }

    /// <summary>
    ///     Create a new settings entry.
    /// </summary>
    IAsyncRelayCommand CreateEntryCommand { get; }

    /// <summary>
    ///     Set the selected settings entry as active.
    /// </summary>
    IRelayCommand<ObservableIniEntry> ActivateEntryCommand { get; }

    /// <summary>
    ///     Delete selected settings entry.
    /// </summary>
    IRelayCommand<ObservableIniEntry> DeleteEntryCommand { get; }

    /// <summary>
    ///     Restore default value for the selected settings entry.
    /// </summary>
    IRelayCommand<ObservableIniEntry> RestoreDefaultCommand { get; }

    /// <summary>
    ///     Task for initializing settings entries.
    /// </summary>
    Task<List<ObservableIniEntry>>? InitializationTask { get; }

    /// <summary>
    ///     Initialize settings entries.
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    ///     Update a settings entry value.
    /// </summary>
    Task UpdateEntryAsync();
}