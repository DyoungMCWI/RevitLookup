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

using System.Windows;
using System.Windows.Automation.Peers;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.UI.Framework.Controls.Automation;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace RevitLookup.UI.Framework.Views.Windows;

public sealed partial class RevitLookupView
{
    private readonly IWindowIntercomService _intercomService;
    private readonly ISoftwareUpdateService _updateService;
    private readonly ISettingsService _settingsService;
    private readonly IThemeWatcherService _themeWatcherService;

    public RevitLookupView(
        INavigationService navigationService,
        IContentDialogService dialogService,
        ISnackbarService snackbarService,
        IWindowIntercomService intercomService,
        ISoftwareUpdateService updateService,
        ISettingsService settingsService,
        IThemeWatcherService themeWatcherService)
    {
        _intercomService = intercomService;
        _updateService = updateService;
        _settingsService = settingsService;
        _themeWatcherService = themeWatcherService;

        themeWatcherService.Watch(this);
        InitializeComponent();

        intercomService.SetSharedHost(this);
        navigationService.SetNavigationControl(RootNavigation);
        dialogService.SetDialogHost(RootContentDialog);
        snackbarService.SetSnackbarPresenter(RootSnackbar);

        ApplyEffects();
        AddShortcuts();
        AddBadges();
        ApplyWindowSize();
        FixComponentsTheme();
    }

    private void AddBadges()
    {
        if (_updateService.NewVersion is null) return;
        if (_updateService.LocalFilePath is not null) return;

        UpdatesNotifier.Visibility = Visibility.Visible;
    }

    private void ApplyEffects()
    {
        WindowBackdropType = _settingsService.ApplicationSettings.Background;
        RootNavigation.Transition = _settingsService.ApplicationSettings.Transition;
        WindowBackgroundManager.UpdateBackground(this, _settingsService.ApplicationSettings.Theme, WindowBackdropType);
    }

    private void ApplyWindowSize()
    {
        if (!_settingsService.ApplicationSettings.UseSizeRestoring) return;

        if (_settingsService.ApplicationSettings.WindowWidth >= MinWidth) Width = _settingsService.ApplicationSettings.WindowWidth;
        if (_settingsService.ApplicationSettings.WindowHeight >= MinHeight) Height = _settingsService.ApplicationSettings.WindowHeight;

        EnableSizeTracking();
    }

    public void EnableSizeTracking()
    {
        SizeChanged += OnSizeChanged;
    }

    public void DisableSizeTracking()
    {
        SizeChanged -= OnSizeChanged;
    }

    private static void OnSizeChanged(object sender, SizeChangedEventArgs args)
    {
        var self = (RevitLookupView) sender;
        self._settingsService.ApplicationSettings.WindowWidth = args.NewSize.Width;
        self._settingsService.ApplicationSettings.WindowHeight = args.NewSize.Height;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new NoAutomationWindowPeer(this);
    }
}