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

using RevitLookup.Abstractions.ObservableModels.Entries;
using RevitLookup.Abstractions.Services.Appearance;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Framework.Views.EditDialogs;

public sealed partial class EditSettingsEntryDialog
{
    private ObservableIniEntry? _entry;

    public EditSettingsEntryDialog(
        IContentDialogService dialogService,
        IThemeWatcherService themeWatcherService)
        : base(dialogService.GetDialogHost())
    {
        InitializeComponent();
        themeWatcherService.Watch(this);
    }

    public ObservableIniEntry Entry
    {
        get => _entry ?? throw new InvalidOperationException("Entry was never set");
        private set => _entry = value;
    }

    public async Task<ContentDialogResult> ShowCreateDialogAsync(ObservableIniEntry? selectedEntry)
    {
        Title = "Create the entry";
        PrimaryButtonText = "Create";

        Entry = new ObservableIniEntry
        {
            IsActive = true
        };

        if (selectedEntry is not null)
        {
            Entry.Category = selectedEntry.Category;
        }

        DataContext = Entry;
        return await ShowAsync();
    }

    public async Task<ContentDialogResult> ShowUpdateDialogAsync(ObservableIniEntry entry)
    {
        Title = "Update the entry";
        PrimaryButtonText = "Update";

        Entry = entry;
        DataContext = entry;
        return await ShowAsync();
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        if (button == ContentDialogButton.Primary)
        {
            Entry.Validate();
            if (Entry.HasErrors)
            {
                return;
            }
        }

        base.OnButtonClick(button);
    }
}