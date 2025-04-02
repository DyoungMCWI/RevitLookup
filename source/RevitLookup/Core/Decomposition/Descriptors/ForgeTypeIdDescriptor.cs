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

using System.Reflection;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class ForgeTypeIdDescriptor : Descriptor, IDescriptorResolver, IDescriptorExtension
{
    private readonly ForgeTypeId _typeId;

    public ForgeTypeIdDescriptor(ForgeTypeId typeId)
    {
        _typeId = typeId;
        Name = typeId.TypeId;
    }

    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(ForgeTypeId.Clear) when parameters.Length == 0 => Variants.Disabled,
            _ => null
        };
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("ToUnitLabel", () => Variants.Value(_typeId.ToUnitLabel()));
        manager.Register("ToSpecLabel", () => Variants.Value(_typeId.ToSpecLabel()));
        manager.Register("ToSymbolLabel", () => Variants.Value(_typeId.ToSymbolLabel()));
#if REVIT2022_OR_GREATER
        manager.Register("ToGroupLabel", () => Variants.Value(_typeId.ToGroupLabel()));
        manager.Register("ToDisciplineLabel", () => Variants.Value(_typeId.ToDisciplineLabel()));
        manager.Register("ToParameterLabel", () => Variants.Value(_typeId.ToParameterLabel()));
#endif
        manager.Register("IsUnit", () => Variants.Value(UnitUtils.IsUnit(_typeId)));
        manager.Register("IsSymbol", () => Variants.Value(UnitUtils.IsSymbol(_typeId)));
#if REVIT2022_OR_GREATER
        manager.Register("IsSpec", () => Variants.Value(SpecUtils.IsSpec(_typeId)));
        manager.Register("IsMeasurableSpec", () => Variants.Value(UnitUtils.IsMeasurableSpec(_typeId)));
        manager.Register("IsBuiltInParameter", () => Variants.Value(ParameterUtils.IsBuiltInParameter(_typeId)));
        manager.Register("IsBuiltInGroup", () => Variants.Value(ParameterUtils.IsBuiltInGroup(_typeId)));
#endif
    }
}