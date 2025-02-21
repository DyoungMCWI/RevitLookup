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

public sealed class DatumPlaneDescriptor(DatumPlane datumPlane) : ElementDescriptor(datumPlane)
{
    public override Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(DatumPlane.CanBeVisibleInView) => ResolveCanBeVisibleInView,
            nameof(DatumPlane.GetPropagationViews) => ResolvePropagationViews,
            nameof(DatumPlane.GetDatumExtentTypeInView) => ResolveDatumExtentTypeInView,
            nameof(DatumPlane.HasBubbleInView) => ResolveHasBubbleInView,
            nameof(DatumPlane.IsBubbleVisibleInView) => ResolveBubbleVisibleInView,
            nameof(DatumPlane.GetCurvesInView) => ResolveGetCurvesInView,
            nameof(DatumPlane.GetLeader) => ResolveGetLeader,
            _ => null
        };

        IVariant ResolveCanBeVisibleInView()
        {
            var views = datumPlane.Document.EnumerateInstances<View>().ToArray();
            var variants = Variants.Values<bool>(views.Length);

            foreach (var view in views)
            {
                var result = datumPlane.CanBeVisibleInView(view);
                variants.Add(result, $"{view.Name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolvePropagationViews()
        {
            var views = datumPlane.Document.EnumerateInstances<View>().ToArray();
            var variants = Variants.Values<ISet<ElementId>>(views.Length);

            foreach (var view in views)
            {
                if (!datumPlane.CanBeVisibleInView(view)) continue;

                var result = datumPlane.GetPropagationViews(view);
                variants.Add(result, view.Name);
            }

            return variants.Consume();
        }

        IVariant ResolveDatumExtentTypeInView()
        {
            var resultEnd0 = datumPlane.GetDatumExtentTypeInView(DatumEnds.End0, Context.ActiveView);
            var resultEnd1 = datumPlane.GetDatumExtentTypeInView(DatumEnds.End1, Context.ActiveView);

            return Variants.Values<DatumExtentType>(2)
                .Add(resultEnd0, $"End 0, Active view: {resultEnd0}")
                .Add(resultEnd1, $"End 1, Active view: {resultEnd1}")
                .Consume();
        }

        IVariant ResolveHasBubbleInView()
        {
            var resultEnd0 = datumPlane.HasBubbleInView(DatumEnds.End0, Context.ActiveView);
            var resultEnd1 = datumPlane.HasBubbleInView(DatumEnds.End1, Context.ActiveView);

            return Variants.Values<bool>(2)
                .Add(resultEnd0, $"End 0, Active view: {resultEnd0}")
                .Add(resultEnd1, $"End 1, Active view: {resultEnd1}")
                .Consume();
        }

        IVariant ResolveBubbleVisibleInView()
        {
            var resultEnd0 = datumPlane.IsBubbleVisibleInView(DatumEnds.End0, Context.ActiveView);
            var resultEnd1 = datumPlane.IsBubbleVisibleInView(DatumEnds.End1, Context.ActiveView);

            return Variants.Values<bool>(2)
                .Add(resultEnd0, $"End 0, Active view: {resultEnd0}")
                .Add(resultEnd1, $"End 1, Active view: {resultEnd1}")
                .Consume();
        }

        IVariant ResolveGetCurvesInView()
        {
            return Variants.Values<IList<Curve>>(2)
                .Add(datumPlane.GetCurvesInView(DatumExtentType.Model, Context.ActiveView), "Model, Active view")
                .Add(datumPlane.GetCurvesInView(DatumExtentType.ViewSpecific, Context.ActiveView), "ViewSpecific, Active view")
                .Consume();
        }

        IVariant ResolveGetLeader()
        {
            return Variants.Values<Leader>(2)
                .Add(datumPlane.GetLeader(DatumEnds.End0, Context.ActiveView), "End 0, Active view")
                .Add(datumPlane.GetLeader(DatumEnds.End1, Context.ActiveView), "End 1, Active view")
                .Consume();
        }
    }

    public override void RegisterExtensions(IExtensionManager manager)
    {
    }
}