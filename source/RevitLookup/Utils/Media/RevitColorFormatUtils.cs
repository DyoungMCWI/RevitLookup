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

using Color = System.Drawing.Color;

namespace RevitLookup.Utils.Media;

/// <summary>
///     Helper class to easier work with color formats
/// </summary>
public static class RevitColorFormatUtils
{
    /// <summary>
    ///     Return a drawing color of a given <see cref="Autodesk.Revit.DB.Color"/>
    /// </summary>
    public static Color GetDrawingColor(this Autodesk.Revit.DB.Color color)
    {
        return Color.FromArgb(1, color.Red, color.Green, color.Blue);
    }
}