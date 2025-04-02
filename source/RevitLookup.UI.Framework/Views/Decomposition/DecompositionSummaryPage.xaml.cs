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

using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.ViewModels.Decomposition;

namespace RevitLookup.UI.Framework.Views.Decomposition;

public sealed partial class DecompositionSummaryPage
{
    public DecompositionSummaryPage(
        IServiceProvider serviceProvider,
        IDecompositionSummaryViewModel viewModel,
        ISettingsService settingsService,
        IWindowIntercomService intercomService,
        INotificationService notificationService,
        IThemeWatcherService themeWatcherService,
        ILoggerFactory loggerFactory)
        : base(serviceProvider, settingsService, intercomService, notificationService, loggerFactory)
    {
        themeWatcherService.Watch(this);

        DataContext = this;
        ViewModel = viewModel;
        InitializeComponent();

        SearchBoxControl = SummarySearchBox;
        TreeViewControl = SummaryTreeView;
        DataGridControl = SummaryDataGrid;
        InitializeControls();
    }
}