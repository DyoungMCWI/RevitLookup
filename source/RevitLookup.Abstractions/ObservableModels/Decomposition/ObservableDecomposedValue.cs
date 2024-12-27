﻿using CommunityToolkit.Mvvm.ComponentModel;
using LookupEngine.Abstractions.Descriptors;

namespace RevitLookup.Abstractions.ObservableModels.Decomposition;

public sealed class ObservableDecomposedValue : ObservableObject
{
    public required object? RawValue { get; init; }
    public required string Name { get; set; }
    public required string TypeName { get; set; }
    public required string TypeFullName { get; set; }
    public string? Description { get; set; }
    public Descriptor? Descriptor { get; set; }
}