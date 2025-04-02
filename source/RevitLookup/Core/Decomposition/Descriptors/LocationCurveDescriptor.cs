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
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class LocationCurveDescriptor(LocationCurve locationCurve) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            "ElementsAtJoin" => ResolveElementsAtJoin,
            "JoinType" => ResolveJoinType,
            _ => null
        };

        IVariant ResolveElementsAtJoin()
        {
            var variants = Variants.Values<ElementArray>(2);
            for (var i = 0; i < 2; i++)
            {
                var elements = locationCurve.get_ElementsAtJoin(i);
                variants.Add(elements, $"Point {i}");
            }

            return variants.Consume();
        }

        IVariant ResolveJoinType()
        {
            var variants = Variants.Values<JoinType>(2);
            for (var i = 0; i < 2; i++)
            {
                var joinType = locationCurve.get_JoinType(i);
                variants.Add(joinType, $"Point {i}");
            }

            return variants.Consume();
        }
    }
}