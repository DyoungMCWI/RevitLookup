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

using CommunityToolkit.Mvvm.Input;
using RevitLookup.Abstractions.Models.UserInterface;

namespace RevitLookup.Abstractions.ViewModels.Dashboard;

/// <summary>
///     Represents the data for the Dashboard view.
/// </summary>
public interface IDashboardViewModel
{
    /// <summary>
    ///     The list of navigation actions.
    /// </summary>
    List<NavigationCardGroup> NavigationGroups { get; }
    
    /// <summary>
    ///     Navigate to a specific page.
    /// </summary>
    IAsyncRelayCommand<string?> NavigatePageCommand { get; }
    
    /// <summary>
    ///     Open a dialog for an action.
    /// </summary>
    IAsyncRelayCommand<string?> OpenDialogCommand { get; }
}