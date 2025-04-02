// Copyright (c) Lookup Foundation and Contributors
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// THIS PROGRAM IS PROVIDED "AS IS" AND WITH ALL FAULTS.
// NO IMPLIED WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE IS PROVIDED.
// THERE IS NO GUARANTEE THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.

using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Abstractions.ViewModels.Settings;

/// <summary>
///     Represents the data for the Settings view.
/// </summary>
public interface ISettingsViewModel
{
    /// <summary>
    ///     The application theme.
    /// </summary>
    ApplicationTheme Theme { get; set; }

    /// <summary>
    ///     The list of available themes.
    /// </summary>
    List<ApplicationTheme> Themes { get; }

    /// <summary>
    ///     The window background effect.
    /// </summary>
    WindowBackdropType Background { get; set; }

    /// <summary>
    ///     The list of available background effects.
    /// </summary>
    List<WindowBackdropType> BackgroundEffects { get; }

    /// <summary>
    ///     Whether to use transition animations.
    /// </summary>
    bool UseTransition { get; set; }

    /// <summary>
    ///     Whether to use hardware rendering.
    /// </summary>
    bool UseHardwareRendering { get; set; }

    /// <summary>
    ///     Whether to restore window initial size.
    /// </summary>
    bool UseSizeRestoring { get; set; }

    /// <summary>
    ///     Whether to use the Revit Modify tab.
    /// </summary>
    bool UseModifyTab { get; set; }

    /// <summary>
    ///     Reset settings to default values.
    /// </summary>
    IAsyncRelayCommand ResetSettingsCommand { get; }
}