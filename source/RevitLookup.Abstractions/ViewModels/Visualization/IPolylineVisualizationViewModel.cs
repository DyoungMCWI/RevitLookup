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
///     Represents the data for polyline visualization.
/// </summary>
public interface IPolylineVisualizationViewModel
{
    /// <summary>
    ///     The minimum thickness of polyline.
    /// </summary>
    double MinThickness { get; }

    /// <summary>
    ///     The diameter of polyline.
    /// </summary>
    double Diameter { get; set; }

    /// <summary>
    ///     The transparency level of visualization.
    /// </summary>
    double Transparency { get; set; }

    /// <summary>
    ///     The color of polyline surface.
    /// </summary>
    Color SurfaceColor { get; set; }

    /// <summary>
    ///     The color of polyline curve.
    /// </summary>
    Color CurveColor { get; set; }

    /// <summary>
    ///     The color of polyline direction indicators.
    /// </summary>
    Color DirectionColor { get; set; }

    /// <summary>
    ///     Whether to show polyline surface.
    /// </summary>
    bool ShowSurface { get; set; }

    /// <summary>
    ///     Whether to show polyline curve.
    /// </summary>
    bool ShowCurve { get; set; }

    /// <summary>
    ///     Whether to show polyline direction indicators.
    /// </summary>
    bool ShowDirection { get; set; }

    /// <summary>
    ///     Register Polyline visualization server.
    /// </summary>
    public void RegisterServer(object curveOrEdge);

    /// <summary>
    ///     Unregister Polyline visualization server.
    /// </summary>
    public void UnregisterServer();
}