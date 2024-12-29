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

using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Summary.Descriptors;

public sealed class MeshDescriptor(Mesh mesh) : Descriptor, IDescriptorCollector
{
    // public void RegisterMenu(ContextMenu contextMenu)
    // {
    //     contextMenu.AddMenuItem("VisualizeMenuItem")
    //         .SetAvailability(mesh.Vertices.Count > 0)
    //         .SetCommand(mesh, async meshArg =>
    //         {
    //             if (Context.ActiveUiDocument is null) return;
    //
    //             var context = (ISnoopViewModel) contextMenu.DataContext;
    //
    //             try
    //             {
    //                 var dialog = context.ServiceProvider.GetRequiredService<MeshVisualizationDialog>();
    //                 await dialog.ShowDialogAsync(meshArg);
    //             }
    //             catch (Exception exception)
    //             {
    //                 var logger = context.ServiceProvider.GetRequiredService<ILogger<MeshDescriptor>>();
    //                 logger.LogError(exception, "VisualizationDialog error");
    //             }
    //         })
    //         .SetShortcut(Key.F8);
    // }
}