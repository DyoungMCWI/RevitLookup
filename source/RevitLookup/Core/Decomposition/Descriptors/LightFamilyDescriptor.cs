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
using Autodesk.Revit.DB.Lighting;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class LightFamilyDescriptor(LightFamily lightFamily) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(LightFamily.GetLightTypeName) => ResolveLightTypeName,
            nameof(LightFamily.GetLightType) => ResolveLightType,
            _ => null
        };

        IVariant ResolveLightTypeName()
        {
            var capacity = lightFamily.GetNumberOfLightTypes();
            var variants = Variants.Values<string>(capacity);
            for (var i = 0; i < capacity; i++)
            {
                var name = lightFamily.GetLightTypeName(i);
                variants.Add(name);
            }

            return variants.Consume();
        }

        IVariant ResolveLightType()
        {
            var capacity = lightFamily.GetNumberOfLightTypes();
            var variants = Variants.Values<LightType>(capacity);
            for (var i = 0; i < capacity; i++)
            {
                var type = lightFamily.GetLightType(i);
                variants.Add(type, $"Index {i}");
            }

            return variants.Consume();
        }
    }
}