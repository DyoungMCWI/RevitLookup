// Copyright 2003-2024 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Reflection;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class IndependentTagDescriptor(IndependentTag tag) : ElementDescriptor(tag)
{
    public override Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(IndependentTag.CanLeaderEndConditionBeAssigned) => ResolveLeaderEndCondition,
#if REVIT2022_OR_GREATER
            nameof(IndependentTag.GetLeaderElbow) => ResolveLeaderElbow,
            nameof(IndependentTag.GetLeaderEnd) => ResolveLeaderEnd,
            nameof(IndependentTag.HasLeaderElbow) => ResolveHasLeaderElbow,
#endif
#if REVIT2023_OR_GREATER
            nameof(IndependentTag.IsLeaderVisible) => ResolveIsLeaderVisible,
#endif
            _ => null
        };

        IVariant ResolveLeaderEndCondition()
        {
            var conditions = Enum.GetValues(typeof(LeaderEndCondition));
            var variants = Variants.Values<bool>(conditions.Length);

            foreach (LeaderEndCondition condition in conditions)
            {
                var result = tag.CanLeaderEndConditionBeAssigned(condition);
                variants.Add(result, $"{condition}: {result}");
            }

            return variants.Consume();
        }
#if REVIT2022_OR_GREATER
        IVariant ResolveLeaderElbow()
        {
            var references = tag.GetTaggedReferences();
            var variants = Variants.Values<XYZ>(references.Count);

            foreach (var reference in references)
            {
#if REVIT2023_OR_GREATER
                if (!tag.IsLeaderVisible(reference)) continue;
#endif
                if (!tag.HasLeaderElbow(reference)) continue;

                variants.Add(tag.GetLeaderElbow(reference));
            }

            return variants.Consume();
        }

        IVariant ResolveLeaderEnd()
        {
            var references = tag.GetTaggedReferences();
            var variants = Variants.Values<XYZ>(references.Count);

            foreach (var reference in references)
            {
#if REVIT2023_OR_GREATER
                if (!tag.IsLeaderVisible(reference)) continue;
#endif
                variants.Add(tag.GetLeaderEnd(reference));
            }

            return variants.Consume();
        }

        IVariant ResolveHasLeaderElbow()
        {
            var references = tag.GetTaggedReferences();
            var variants = Variants.Values<bool>(references.Count);
            foreach (var reference in references)
            {
#if REVIT2023_OR_GREATER
                if (!tag.IsLeaderVisible(reference)) continue;
#endif
                variants.Add(tag.HasLeaderElbow(reference));
            }

            return variants.Consume();
        }
#endif
#if REVIT2023_OR_GREATER

        IVariant ResolveIsLeaderVisible()
        {
            var references = tag.GetTaggedReferences();
            var variants = Variants.Values<bool>(references.Count);

            foreach (var reference in references)
            {
                variants.Add(tag.IsLeaderVisible(reference));
            }

            return variants.Consume();
        }
#endif
    }

    public override void RegisterExtensions(IExtensionManager manager)
    {
    }
}