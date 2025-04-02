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
using Autodesk.Revit.DB.Mechanical;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using ArgumentException = Autodesk.Revit.Exceptions.ArgumentException;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class MepSectionDescriptor(MEPSection mepSection) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(MEPSection.GetElementIds) => ResolveSectionIds,
            nameof(MEPSection.GetCoefficient) => ResolveCoefficient,
            nameof(MEPSection.GetPressureDrop) => ResolvePressureDrop,
            nameof(MEPSection.GetSegmentLength) => ResolveSegmentLength,
            nameof(MEPSection.IsMain) => ResolveIsMain,
            _ => null
        };

        IVariant ResolveSectionIds()
        {
            var elementIds = mepSection.GetElementIds();
            var variants = Variants.Values<ElementId>(elementIds.Count);
            foreach (var id in elementIds)
            {
                variants.Add(id);
            }

            return variants.Consume();
        }

        IVariant ResolveCoefficient()
        {
            var elementIds = mepSection.GetElementIds();
            var variants = Variants.Values<double>(elementIds.Count);
            foreach (var id in elementIds)
            {
                variants.Add(mepSection.GetCoefficient(id), $"ID{id}");
            }

            return variants.Consume();
        }

        IVariant ResolvePressureDrop()
        {
            var elementIds = mepSection.GetElementIds();
            var variants = Variants.Values<double>(elementIds.Count);
            foreach (var id in elementIds)
            {
                variants.Add(mepSection.GetPressureDrop(id), $"ID{id}");
            }

            return variants.Consume();
        }

        IVariant ResolveSegmentLength()
        {
            var elementIds = mepSection.GetElementIds();
            var variants = Variants.Values<double>(elementIds.Count);
            foreach (var id in elementIds)
            {
                try
                {
                    var length = mepSection.GetSegmentLength(id);
                    variants.Add(length, $"ID{id}");
                }
                catch (ArgumentException)
                {
                    // ignored
                }
            }

            return variants.Consume();
        }

        IVariant ResolveIsMain()
        {
            var elementIds = mepSection.GetElementIds();
            var variants = Variants.Values<bool>(elementIds.Count);
            foreach (var id in elementIds)
            {
                try
                {
                    var isMain = mepSection.IsMain(id);
                    variants.Add(isMain, $"ID{id}");
                }
                catch (ArgumentException)
                {
                    // ignored
                }
            }

            return variants.Consume();
        }
    }
}