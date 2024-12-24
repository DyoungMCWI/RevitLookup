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

using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.ViewModels.Settings;
using RevitLookup.Services.Application;
using RevitLookup.UI.Framework.Views.Settings;
using RevitLookup.UI.Framework.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.Animations;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using ApplicationTheme = Wpf.Ui.Appearance.ApplicationTheme;

namespace RevitLookup.ViewModels.Settings;

[UsedImplicitly]
public sealed partial class SettingsViewModel : ObservableObject, ISettingsViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly IWindowIntercomService _intercomService;
    private readonly IThemeWatcherService _themeWatcherService;
    private readonly RevitRibbonService _ribbonService;
    private readonly bool _initialized;

    [ObservableProperty] private ApplicationTheme _theme;
    [ObservableProperty] private WindowBackdropType _background;

    [ObservableProperty] private bool _useTransition;
    [ObservableProperty] private bool _useHardwareRendering;
    [ObservableProperty] private bool _useSizeRestoring;
    [ObservableProperty] private bool _useModifyTab;

    public SettingsViewModel(
        IServiceProvider serviceProvider,
        INavigationService navigationService,
        INotificationService notificationService,
        ISettingsService settingsService,
        IWindowIntercomService intercomService,
        IThemeWatcherService themeWatcherService,
        RevitRibbonService ribbonService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _notificationService = notificationService;
        _settingsService = settingsService;
        _intercomService = intercomService;
        _themeWatcherService = themeWatcherService;
        _ribbonService = ribbonService;

        ApplySettings();
        _initialized = true;
    }

    public List<ApplicationTheme> Themes { get; } =
    [
        ApplicationTheme.Auto,
        ApplicationTheme.Light,
        ApplicationTheme.Dark
        // ApplicationTheme.HighContrast
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

            if (dialog.CanResetGeneralSettings)
            {
                _settingsService.ResetGeneralSettings();
            }

            if (dialog.CanResetRenderSettings)
            {
                _settingsService.ResetRenderSettings();
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

        _settingsService.GeneralSettings.Theme = value;
        _themeWatcherService.Watch();
    }

    partial void OnThemeChanged(ApplicationTheme oldValue, ApplicationTheme newValue)
    {
        if (!_initialized) return;

        if (oldValue == ApplicationTheme.Auto)
        {
            _themeWatcherService.Unwatch();
        }
    }

    partial void OnBackgroundChanged(WindowBackdropType value)
    {
        if (!_initialized) return;

        _settingsService.GeneralSettings.Background = value;
        WindowBackgroundManager.UpdateBackground(_intercomService.GetHost(), _settingsService.GeneralSettings.Theme, value);
    }

    partial void OnUseTransitionChanged(bool value)
    {
        if (!_initialized) return;

        var navigationControl = _navigationService.GetNavigationControl();
        var transition = _settingsService.GeneralSettings.Transition = value
            ? (Transition) NavigationView.TransitionProperty.DefaultMetadata.DefaultValue
            : Transition.None;

        _settingsService.GeneralSettings.Transition = transition;
        navigationControl.Transition = transition;
    }

    partial void OnUseHardwareRenderingChanged(bool value)
    {
        if (!_initialized) return;

        _settingsService.GeneralSettings.UseHardwareRendering = value;
        if (value) Application.EnableHardwareRendering();
        else Application.DisableHardwareRendering();
    }

    partial void OnUseSizeRestoringChanged(bool value)
    {
        if (!_initialized) return;

        _settingsService.GeneralSettings.UseSizeRestoring = value;
        if (_intercomService.GetHost() is not RevitLookupView lookupView)
        {
            Debug.Fail("Settings page running inside invalid host");
            return;
        }

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

        _settingsService.GeneralSettings.UseModifyTab = value;
        _ribbonService.CreateRibbon();
    }

    private void ApplySettings()
    {
        Theme = _settingsService.GeneralSettings.Theme;
        Background = _settingsService.GeneralSettings.Background;
        UseTransition = _settingsService.GeneralSettings.Transition != Transition.None;
        UseHardwareRendering = _settingsService.GeneralSettings.UseHardwareRendering;
        UseSizeRestoring = _settingsService.GeneralSettings.UseSizeRestoring;
        UseModifyTab = _settingsService.GeneralSettings.UseModifyTab;
    }
}