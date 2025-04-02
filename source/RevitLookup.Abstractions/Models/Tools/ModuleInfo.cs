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
///     Represents a metadata of an assembly runtime module.
/// </summary>
public sealed class ModuleInfo
{
    /// <summary>
    ///     The module name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     The module file path.
    /// </summary>
    public required string Path { get; init; }

    /// <summary>
    ///     The module load order.
    /// </summary>
    public required int Order { get; init; }

    /// <summary>
    ///     The module version.
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    ///     The module container. Isolation context or domain.
    /// </summary>
    public required string Container { get; init; }
}