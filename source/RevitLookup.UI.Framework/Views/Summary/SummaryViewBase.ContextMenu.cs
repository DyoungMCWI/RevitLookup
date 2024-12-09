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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.UI.Framework.Engine.Configuration;
using RevitLookup.UI.Framework.Extensions;
using RevitLookup.UI.Framework.Utils;
using Wpf.Ui;

namespace RevitLookup.UI.Framework.Views.Summary;

public partial class SummaryViewBase
{
    /// <summary>
    ///     Tree view context menu
    /// </summary>
    private void CreateTreeContextMenu(ObservableDecomposedObject decomposedObject, FrameworkElement row)
    {
        var contextMenu = new ContextMenu
        {
            PlacementTarget = row,
            Resources = UiApplication.Current.Resources
        };

        row.ContextMenu = contextMenu;

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetCommand(decomposedObject, parameter => Clipboard.SetDataObject(parameter.Name))
            .SetShortcut(ModifierKeys.Control, Key.C);
        contextMenu.AddMenuItem("HelpMenuItem")
            .SetCommand(decomposedObject, parameter => HelpUtils.ShowHelp(parameter.TypeFullName))
            .SetShortcut(Key.F1);

        if (decomposedObject.Descriptor is not IDescriptorConnector connector) return;

        try
        {
            connector.RegisterMenu(contextMenu);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to register the context menu");
            _notificationService.ShowError("Failed to register the context menu", exception);
        }
    }

    /// <summary>
    ///     Data grid context menu
    /// </summary>
    private void CreateGridContextMenu(DataGrid dataGrid)
    {
        var contextMenu = new ContextMenu
        {
            PlacementTarget = dataGrid,
            Resources = UiApplication.Current.Resources
        };

        dataGrid.ContextMenu = contextMenu;

        contextMenu.AddMenuItem("RefreshMenuItem")
            .SetCommand(ViewModel.RefreshMembersCommand)
            .SetGestureText(Key.F5);

        contextMenu.AddSeparator();
        contextMenu.AddLabel("Columns");

        contextMenu.AddMenuItem()
            .SetHeader("Time")
            .SetStaysOpenOnClick(true)
            .SetChecked(dataGrid.Columns[2].Visibility == Visibility.Visible)
            .SetCommand(dataGrid.Columns[2], parameter =>
            {
                _settingsService.GeneralSettings.ShowTimeColumn = parameter.Visibility != Visibility.Visible;
                parameter.Visibility = _settingsService.GeneralSettings.ShowTimeColumn ? Visibility.Visible : Visibility.Collapsed;
            });

        contextMenu.AddMenuItem()
            .SetHeader("Memory")
            .SetStaysOpenOnClick(true)
            .SetChecked(dataGrid.Columns[3].Visibility == Visibility.Visible)
            .SetCommand(dataGrid.Columns[3], parameter =>
            {
                _settingsService.GeneralSettings.ShowMemoryColumn = parameter.Visibility != Visibility.Visible;
                parameter.Visibility = _settingsService.GeneralSettings.ShowMemoryColumn ? Visibility.Visible : Visibility.Collapsed;
            });

        contextMenu.AddSeparator();
        contextMenu.AddLabel("Show");

        contextMenu.AddMenuItem()
            .SetHeader("Events")
            .SetStaysOpenOnClick(true)
            .SetChecked(_settingsService.GeneralSettings.IncludeEvents)
            .SetCommand(_settingsService.GeneralSettings, async parameter =>
            {
                parameter.IncludeEvents = !parameter.IncludeEvents;
                await ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Extensions")
            .SetStaysOpenOnClick(true)
            .SetChecked(_settingsService.GeneralSettings.IncludeExtensions)
            .SetCommand(_settingsService.GeneralSettings, async parameter =>
            {
                parameter.IncludeExtensions = !parameter.IncludeExtensions;
                await ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Fields")
            .SetStaysOpenOnClick(true)
            .SetChecked(_settingsService.GeneralSettings.IncludeFields)
            .SetCommand(_settingsService.GeneralSettings, async parameter =>
            {
                parameter.IncludeFields = !parameter.IncludeFields;
                await ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Non-public")
            .SetStaysOpenOnClick(true)
            .SetChecked(_settingsService.GeneralSettings.IncludePrivate)
            .SetCommand(_settingsService.GeneralSettings, async parameter =>
            {
                parameter.IncludePrivate = !parameter.IncludePrivate;
                await ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Root")
            .SetStaysOpenOnClick(true)
            .SetChecked(_settingsService.GeneralSettings.IncludeRootHierarchy)
            .SetCommand(_settingsService.GeneralSettings, async parameter =>
            {
                parameter.IncludeRootHierarchy = !parameter.IncludeRootHierarchy;
                await ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Static")
            .SetStaysOpenOnClick(true)
            .SetChecked(_settingsService.GeneralSettings.IncludeStatic)
            .SetCommand(_settingsService.GeneralSettings, async parameter =>
            {
                parameter.IncludeStatic = !parameter.IncludeStatic;
                await ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
        contextMenu.AddMenuItem()
            .SetHeader("Unsupported")
            .SetStaysOpenOnClick(true)
            .SetChecked(_settingsService.GeneralSettings.IncludeUnsupported)
            .SetCommand(_settingsService.GeneralSettings, async parameter =>
            {
                parameter.IncludeUnsupported = !parameter.IncludeUnsupported;
                await ViewModel.RefreshMembersCommand.ExecuteAsync(null);
            });
    }

    /// <summary>
    ///     Data grid row context menu
    /// </summary>
    private void CreateGridRowContextMenu(ObservableDecomposedMember member, FrameworkElement row)
    {
        if (row.ContextMenu is not null) return;

        var contextMenu = new ContextMenu
        {
            PlacementTarget = row,
            Resources = UiApplication.Current.Resources,
        };

        row.ContextMenu = contextMenu;

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetCommand(member, parameter => Clipboard.SetDataObject($"{parameter.Name}: {parameter.Value.Name}"))
            .SetShortcut(ModifierKeys.Control, Key.C)
            .SetAvailability(member.Value.Name != string.Empty);

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy value")
            .SetCommand(member, parameter => Clipboard.SetDataObject(parameter.Value.Name))
            .SetShortcut(ModifierKeys.Control | ModifierKeys.Shift, Key.C)
            .SetAvailability(member.Value.Name != string.Empty);

        contextMenu.AddMenuItem("HelpMenuItem")
            .SetCommand(member, parameter => HelpUtils.ShowHelp(parameter.DeclaringTypeFullName, parameter.Name))
            .SetShortcut(Key.F1);

        if (member.Value.Descriptor is not IDescriptorConnector connector) return;

        try
        {
            connector.RegisterMenu(contextMenu);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to register the context menu");
            _notificationService.ShowError("Failed to register the context menu", exception);
        }
    }
}