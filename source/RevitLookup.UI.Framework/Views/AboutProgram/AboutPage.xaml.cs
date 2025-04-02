﻿// Copyright (c) Lookup Foundation and Contributors
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
using RevitLookup.Abstractions.ViewModels.AboutProgram;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Framework.Views.AboutProgram;

public sealed partial class AboutPage : INavigableView<IAboutViewModel>
{
    public AboutPage(IAboutViewModel viewModel, IThemeWatcherService themeWatcherService)
    {
        themeWatcherService.Watch(this);

        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public IAboutViewModel ViewModel { get; }
}