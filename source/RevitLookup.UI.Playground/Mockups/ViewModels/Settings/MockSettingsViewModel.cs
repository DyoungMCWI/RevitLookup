// Copyright 2003-2024 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Windows.Interop;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.ViewModels.Settings;
using RevitLookup.UI.Framework.Views.Settings;
using RevitLookup.UI.Framework.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.Animations;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.Mockups.ViewModels.Settings;

[UsedImplicitly]
public sealed partial class MockSettingsViewModel : ObservableObject, ISettingsViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly IThemeWatcherService _themeWatcherService;
    private readonly IWindowIntercomService _intercomService;
    private readonly bool _initialized;

    [ObservableProperty] private ApplicationTheme _theme;
    [ObservableProperty] private WindowBackdropType _background;

    [ObservableProperty] private bool _useTransition;
    [ObservableProperty] private bool _useHardwareRendering;
    [ObservableProperty] private bool _useSizeRestoring;
    [ObservableProperty] private bool _useModifyTab;

    public MockSettingsViewModel(
        IServiceProvider serviceProvider,
        INavigationService navigationService,
        INotificationService notificationService,
        ISettingsService settingsService,
        IThemeWatcherService themeWatcherService,
        IWindowIntercomService intercomService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _notificationService = notificationService;
        _settingsService = settingsService;
        _themeWatcherService = themeWatcherService;
        _intercomService = intercomService;

        ApplySettings();
        _initialized = true;
    }

    public List<ApplicationTheme> Themes { get; } =
    [
        ApplicationTheme.Auto,
        ApplicationTheme.Light,
        ApplicationTheme.Dark,
        ApplicationTheme.HighContrast
    ];

    public List<WindowBackdropType> BackgroundEffects { get; } =
    [
        WindowBackdropType.None,
        WindowBackdropType.Acrylic,
        WindowBackdropType.Tabbed,
        WindowBackdropType.Mica
    ];

    [RelayCommand]
    private async Task ResetSettings()
    {
        try
        {
            var dialog = _serviceProvider.GetRequiredService<ResetSettingsDialog>();
            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary) return;

            if (dialog.CanResetApplicationSettings)
            {
                _settingsService.ResetApplicationSettings();
            }

            if (dialog.CanResetVisualizationSettings)
            {
                _settingsService.ResetVisualizationSettings();
            }

            ApplySettings();
            _notificationService.ShowSuccess("Reset settings", "Settings successfully reset to default");
        }
        catch (Exception exception)
        {
            _notificationService.ShowError("Reset settings error", exception);
        }
    }

    partial void OnThemeChanged(ApplicationTheme value)
    {
        if (!_initialized) return;

        _settingsService.ApplicationSettings.Theme = value;
        _themeWatcherService.ApplyTheme();
    }

    partial void OnBackgroundChanged(WindowBackdropType value)
    {
        if (!_initialized) return;

        _settingsService.ApplicationSettings.Background = value;
        WindowBackgroundManager.UpdateBackground(_intercomService.GetHost(), _settingsService.ApplicationSettings.Theme, value);
    }

    partial void OnUseTransitionChanged(bool value)
    {
        if (!_initialized) return;

        var navigationControl = _navigationService.GetNavigationControl();
        var transition = _settingsService.ApplicationSettings.Transition = value
            ? (Transition) NavigationView.TransitionProperty.DefaultMetadata.DefaultValue
            : Transition.None;

        _settingsService.ApplicationSettings.Transition = transition;
        navigationControl.Transition = transition;
    }

    partial void OnUseHardwareRenderingChanged(bool value)
    {
        if (!_initialized) return;

        _settingsService.ApplicationSettings.UseHardwareRendering = value;
        RenderOptions.ProcessRenderMode = value ? RenderMode.Default : RenderMode.SoftwareOnly;
    }

    partial void OnUseSizeRestoringChanged(bool value)
    {
        if (!_initialized) return;

        _settingsService.ApplicationSettings.UseSizeRestoring = value;
        if (_intercomService.GetHost() is not RevitLookupView lookupView) return;

        if (value)
        {
            lookupView.EnableSizeTracking();
        }
        else
        {
            lookupView.DisableSizeTracking();
        }
    }

    partial void OnUseModifyTabChanged(bool value)
    {
        if (!_initialized) return;

        _settingsService.ApplicationSettings.UseModifyTab = value;
    }

    private void ApplySettings()
    {
        Theme = _settingsService.ApplicationSettings.Theme;
        Background = _settingsService.ApplicationSettings.Background;
        UseTransition = _settingsService.ApplicationSettings.Transition != Transition.None;
        UseHardwareRendering = _settingsService.ApplicationSettings.UseHardwareRendering;
        UseSizeRestoring = _settingsService.ApplicationSettings.UseSizeRestoring;
        UseModifyTab = _settingsService.ApplicationSettings.UseModifyTab;
    }
}