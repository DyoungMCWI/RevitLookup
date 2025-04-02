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
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Framework.Views.EditDialogs;

public sealed partial class EditValueDialog
{
    public EditValueDialog(IContentDialogService dialogService, IThemeWatcherService themeWatcherService) : base(dialogService.GetDialogHost())
    {
        InitializeComponent();
        themeWatcherService.Watch(this);
    }

    public async Task<ContentDialogResult> ShowAsync(string name, string value)
    {
        ValueLabel.Content = name;
        ValueBox.Text = value;
        ValueBox.PlaceholderText = value;

        return await ShowAsync();
    }

    public async Task<ContentDialogResult> ShowAsync(string name, string value, string caption)
    {
        Title = caption;

        ValueLabel.Content = name;
        ValueBox.Text = value;
        ValueBox.PlaceholderText = value;

        return await ShowAsync();
    }

    public string Value => ValueBox.Text;
}