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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using RevitLookup.Abstractions.ObservableModels.Entries;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.ViewModels.Tools;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace RevitLookup.UI.Framework.Views.Tools;

public sealed partial class RevitSettingsPage
{
    private readonly INotificationService _notificationService;

    public RevitSettingsPage(
        IRevitSettingsViewModel viewModel,
        IContentDialogService dialogService,
        INavigationService navigationService,
        IThemeWatcherService themeWatcherService,
        INotificationService notificationService)
    {
        _notificationService = notificationService;

        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
        ApplyGrouping();
        themeWatcherService.Watch(this);

        if (viewModel.Entries.Count == 0)
        {
            ShowWarningDialog(dialogService, navigationService);
        }
    }

    private void ApplyGrouping()
    {
        EntriesList.Items.GroupDescriptions!.Clear();
        EntriesList.Items.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ObservableIniEntry.Category)));
    }

    public IRevitSettingsViewModel ViewModel { get; }

    private async void ShowWarningDialog(IContentDialogService dialogService, INavigationService navigationService)
    {
        try
        {
            var options = new SimpleContentDialogCreateOptions
            {
                Title = "Proceed with caution",
                Content = "Changing advanced configuration preferences can impact Revit performance or security",
                PrimaryButtonText = "Accept the Risk and Continue",
                CloseButtonText = "Quit"
            };

            var result = await dialogService.ShowSimpleDialogAsync(options);
            if (result != ContentDialogResult.Primary)
            {
                navigationService.GoBack();
            }
            else
            {
                await ViewModel.InitializeAsync();
            }
        }
        catch (Exception exception)
        {
            _notificationService.ShowError("Initialization error", exception.Message);
        }
    }

    private async void OnEntryClicked(object sender, MouseButtonEventArgs args)
    {
        try
        {
            if (args.OriginalSource is ButtonBase) return;

            await ViewModel.UpdateEntryAsync();
        }
        catch (Exception exception)
        {
            _notificationService.ShowError("Entry updating error", exception.Message);
        }
    }

    private void OnFilterClicked(object sender, RoutedEventArgs args)
    {
        FilterFlyout.IsOpen = !FilterFlyout.IsOpen;
    }
}