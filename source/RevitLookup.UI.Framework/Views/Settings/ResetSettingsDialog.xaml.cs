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

using RevitLookup.Abstractions.Services.Appearance;
using Wpf.Ui;

namespace RevitLookup.UI.Framework.Views.Settings;

public sealed partial class ResetSettingsDialog
{
    public ResetSettingsDialog(IContentDialogService dialogService, IThemeWatcherService themeWatcherService) : base(dialogService.GetDialogHost())
    {
        InitializeComponent();
        themeWatcherService.Watch(this);
    }

    public bool CanResetApplicationSettings => ApplicationBox.IsChecked == true;
    public bool CanResetDecompositionSettings => DecompositionBox.IsChecked == true;
    public bool CanResetVisualizationSettings => VisualizationBox.IsChecked == true;
}