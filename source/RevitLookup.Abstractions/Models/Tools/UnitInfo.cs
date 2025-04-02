// Copyright (c) Lookup Foundation and Contributors
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

namespace RevitLookup.Abstractions.Models.Tools;

/// <summary>
///     Represents information about the Revit unit.
/// </summary>
public sealed class UnitInfo
{
    /// <summary>
    ///     The unit name.
    /// </summary>
    public required string Unit { get; init; }

    /// <summary>
    ///     The unit label.
    /// </summary>
    public required string Label { get; init; }

    /// <summary>
    ///     The unit value.
    /// </summary>
    public required object Value { get; init; }

    /// <summary>
    ///     The unit class.
    /// </summary>
    public string? Class { get; init; }
}