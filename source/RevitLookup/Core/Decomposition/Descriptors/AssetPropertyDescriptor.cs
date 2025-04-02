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

using System.Reflection;
using Autodesk.Revit.DB.Visual;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class AssetPropertyDescriptor(AssetProperty assetProperty) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(AssetProperty.GetTypeName) => ResolveGetTypeName,
            nameof(AssetProperty.GetConnectedProperty) => ResolveConnectedProperty,
            _ => null
        };

        IVariant ResolveConnectedProperty()
        {
            var capacity = assetProperty.NumberOfConnectedProperties;
            var variants = Variants.Values<AssetProperty>(capacity);
            for (var i = 0; i < capacity; i++)
            {
                var property = assetProperty.GetConnectedProperty(i);
                variants.Add(property, property.Name);
            }

            return variants.Consume();
        }

        IVariant ResolveGetTypeName()
        {
            return Variants.Value(AssetProperty.GetTypeName(assetProperty.Type));
        }
    }
}