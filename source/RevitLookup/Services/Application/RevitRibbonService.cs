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

using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Commands;
using RevitLookup.Commands.Controllers;

namespace RevitLookup.Services.Application;

public sealed class RevitRibbonService(ISettingsService settingsService)
{
    private readonly ActionEventHandler _eventHandler = new();
    private readonly List<RibbonPanel> _createdPanels = new(2);

    public void CreateRibbon()
    {
        _eventHandler.Raise(_ =>
        {
            RemovePanels();
            CreatePanels();
        });
    }

    private void CreatePanels()
    {
        var application = Context.UiControlledApplication;
        var addinsPanel = application.CreatePanel("Revit Lookup");
        var pullButton = addinsPanel.AddPullDownButton("RevitLookupButton", "RevitLookup");
        pullButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        pullButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        pullButton.AddPushButton<ShowDashboardCommand>("Dashboard")
            .SetAvailabilityController<CommandAlwaysAvailableController>();

        if (!settingsService.ApplicationSettings.UseModifyTab)
        {
            pullButton.AddPushButton<DecomposeSelectionCommand>("Snoop Selection")
                .AddShortcuts("SS");
        }

        pullButton.AddPushButton<DecomposeViewCommand>("Snoop Active view");
        pullButton.AddPushButton<DecomposeDocumentCommand>("Snoop Document");
        pullButton.AddPushButton<DecomposeDatabaseCommand>("Snoop Database");
        pullButton.AddPushButton<DecomposeFaceCommand>("Snoop Face");
        pullButton.AddPushButton<DecomposeEdgeCommand>("Snoop Edge");
        pullButton.AddPushButton<DecomposePointCommand>("Snoop Point");
        pullButton.AddPushButton<DecomposeLinkedElementCommand>("Snoop Linked element");
        pullButton.AddPushButton<SearchElementsCommand>("Search Elements");
        pullButton.AddPushButton<ShowEventMonitorCommand>("Event monitor")
            .SetAvailabilityController<CommandAlwaysAvailableController>();

        if (settingsService.ApplicationSettings.UseModifyTab)
        {
            var modifyPanel = application.CreatePanel("Revit Lookup", "Modify");
            modifyPanel.AddPushButton<DecomposeSelectionCommand>("\u00a0Snoop\u00a0\nSelection")
                .AddShortcuts("SS")
                .SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png")
                .SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

            _createdPanels.Add(modifyPanel);
        }

        _createdPanels.Add(addinsPanel);
    }

    private void RemovePanels()
    {
        if (_createdPanels.Count == 0) return;

        foreach (var ribbonPanel in _createdPanels)
        {
            ribbonPanel.RemovePanel();
        }

        _createdPanels.Clear();
    }
}