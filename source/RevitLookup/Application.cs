// Copyright 2003-2024 by Autodesk, Inc. 
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

using System.Windows.Interop;
using System.Windows.Media;
using Nice3point.Revit.Toolkit.External;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Core;
using RevitLookup.Services.Application;

namespace RevitLookup;

[UsedImplicitly]
public class Application : ExternalApplication
{
    public override void OnStartup()
    {
        Host.Start();
        RevitShell.RegisterHandlers();

        EnableThemes();
        EnableHardwareRendering();

        var ribbonService = Host.GetService<RevitRibbonService>();
        ribbonService.CreateRibbon();
    }

    public override void OnShutdown()
    {
        Host.Stop();
    }

    private static void EnableThemes()
    {
        var uiService = Host.GetService<IUiOrchestratorService>();
        uiService.RunService<IThemeWatcherService>(themeService =>
        {
            themeService.Initialize();
            themeService.ApplyTheme();
        });
    }

    public static void EnableHardwareRendering()
    {
        var settingsService = Host.GetService<ISettingsService>();
        if (!settingsService.ApplicationSettings.UseHardwareRendering) return;

        //Revit overrides render mode during initialization
        //EventHandler is called after initialization
        RevitShell.ActionEventHandler.Raise(_ => RenderOptions.ProcessRenderMode = RenderMode.Default);
    }

    public static void DisableHardwareRendering()
    {
        var settingsService = Host.GetService<ISettingsService>();
        if (settingsService.ApplicationSettings.UseHardwareRendering) return;

        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
    }
}