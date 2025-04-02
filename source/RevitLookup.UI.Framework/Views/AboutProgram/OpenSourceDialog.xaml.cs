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
using System.Windows.Documents;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.ViewModels.AboutProgram;
using RevitLookup.Common.Tools;
using Wpf.Ui;

namespace RevitLookup.UI.Framework.Views.AboutProgram;

public sealed partial class OpenSourceDialog
{
    public OpenSourceDialog(
        IContentDialogService dialogService,
        IOpenSourceViewModel viewModel,
        IThemeWatcherService themeWatcherService)
        : base(dialogService.GetDialogHost())
    {
        DataContext = viewModel;
        InitializeComponent();

        themeWatcherService.Watch(this);
    }

    private void OpenLink(object sender, RoutedEventArgs args)
    {
        var link = (Hyperlink) args.OriginalSource;
        ProcessTasks.StartShell(link.NavigateUri.OriginalString);
    }
}