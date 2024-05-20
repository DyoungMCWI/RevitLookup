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

using System.Windows.Media;
using Autodesk.Revit.DB.ExternalService;
using RevitLookup.Core.Servers;

namespace RevitLookup.ViewModels.Dialogs;

public sealed partial class VisualizationViewModel(Face face) : ObservableObject
{
    private readonly FaceVisualizationServer _server = new(face);
    
    [ObservableProperty] private string _color = Colors.Coral.ToString();
    
    public void RegisterServer()
    {
        Application.ActionEventHandler.Raise(application =>
        {
            var directContextService = (MultiServerService) ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.DirectContext3DService);
            var serverIds = directContextService.GetActiveServerIds();
            
            directContextService.AddServer(_server);
            serverIds.Add(_server.GetServerId());
            directContextService.SetActiveServers(serverIds);
            
            application.ActiveUIDocument!.UpdateAllOpenViews();
        });
    }
    
    public void UnregisterServer()
    {
        Application.ActionEventHandler.Raise(application =>
        {
            var directContextService = (MultiServerService) ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.DirectContext3DService);
            directContextService.RemoveServer(_server.GetServerId());
            
            application.ActiveUIDocument!.UpdateAllOpenViews();
        });
    }
}