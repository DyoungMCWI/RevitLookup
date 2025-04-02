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

public sealed class HostObjectDescriptor(HostObject hostObject) : ElementDescriptor(hostObject)
{
    public override Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(HostObject.FindInserts) => ResolveFindInserts,
            _ => null
        };

        IVariant ResolveFindInserts()
        {
            return Variants.Value(hostObject.FindInserts(true, true, true, true));
        }
    }

    public override void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(HostObjectUtils.GetBottomFaces), () => Variants.Value(hostObject.GetBottomFaces()));
        manager.Register(nameof(HostObjectUtils.GetTopFaces), () => Variants.Value(hostObject.GetTopFaces()));
        manager.Register(nameof(HostObjectUtils.GetSideFaces), () => Variants.Values<IList<Reference>>(2)
            .Add(hostObject.GetSideFaces(ShellLayerType.Interior), "Interior")
            .Add(hostObject.GetSideFaces(ShellLayerType.Exterior), "Exterior")
            .Consume());
    }
}