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

using System.Windows.Media;

namespace RevitLookup.Abstractions.ViewModels.Visualization;

/// <summary>
///     Represents the data for mesh visualization.
/// </summary>
public interface IMeshVisualizationViewModel
{
    /// <summary>
    ///     The minimum extrusion value for mesh.
    /// </summary>
    public double MinExtrusion { get; }

    /// <summary>
    ///     The extrusion value for mesh.
    /// </summary>
    double Extrusion { get; set; }

    /// <summary>
    ///     The transparency level of visualization.
    /// </summary>
    double Transparency { get; set; }

    /// <summary>
    ///     The color of mesh surface.
    /// </summary>
    Color SurfaceColor { get; set; }

    /// <summary>
    ///     The color of mesh grid.
    /// </summary>
    Color MeshColor { get; set; }

    /// <summary>
    ///     The color of mesh normal vectors.
    /// </summary>
    Color NormalVectorColor { get; set; }

    /// <summary>
    ///     Whether to show mesh surface.
    /// </summary>
    bool ShowSurface { get; set; }

    /// <summary>
    ///     Whether to show mesh grid.
    /// </summary>
    bool ShowMeshGrid { get; set; }

    /// <summary>
    ///     Whether to show mesh normal vectors.
    /// </summary>
    bool ShowNormalVector { get; set; }

    /// <summary>
    ///     Register Mesh visualization server.
    /// </summary>
    public void RegisterServer(object mesh);

    /// <summary>
    ///     Unregister Mesh visualization server.
    /// </summary>
    public void UnregisterServer();
}