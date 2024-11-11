﻿// Copyright 2003-2024 by Autodesk, Inc.
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

using RevitLookup.Abstractions.ViewModels.Visualization;
using Wpf.Ui;

namespace RevitLookup.UI.Framework.Views.Visualization;

public sealed partial class FaceVisualizationDialog
{
    private readonly IFaceVisualizationViewModel _viewModel;

    public FaceVisualizationDialog(
        IContentDialogService dialogService,
        IFaceVisualizationViewModel viewModel)
        : base(dialogService.GetDialogHost())
    {
        _viewModel = viewModel;

        DataContext = _viewModel;
        InitializeComponent();
    }

    public async Task ShowDialogAsync(object face)
    {
        _viewModel.RegisterServer(face);
        MonitorServerConnection();

        await ShowAsync();
    }

    private void MonitorServerConnection()
    {
        Unloaded += (_, _) => _viewModel.UnregisterServer();
    }
}