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

public sealed class PartDescriptor(Part part) : ElementDescriptor(part), IDescriptorExtension<Document>
{
    public override Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return null;
    }

    public override void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(PartUtils.IsMergedPart), () => Variants.Value(PartUtils.IsMergedPart(part)));
        manager.Register(nameof(PartUtils.IsPartDerivedFromLink), () => Variants.Value(PartUtils.IsPartDerivedFromLink(part)));
        manager.Register(nameof(PartUtils.GetChainLengthToOriginal), () => Variants.Value(PartUtils.GetChainLengthToOriginal(part)));
        manager.Register(nameof(PartUtils.GetMergedParts), () => Variants.Value(PartUtils.GetMergedParts(part)));
    }

    public void RegisterExtensions(IExtensionManager<Document> manager)
    {
        manager.Register(nameof(PartUtils.ArePartsValidForDivide), context => Variants.Value(PartUtils.ArePartsValidForDivide(context, [part.Id])));
        manager.Register(nameof(PartUtils.FindMergeableClusters), context => Variants.Value(PartUtils.FindMergeableClusters(context, [part.Id])));
        manager.Register(nameof(PartUtils.ArePartsValidForMerge), context => Variants.Value(PartUtils.ArePartsValidForMerge(context, [part.Id])));
        manager.Register(nameof(PartUtils.GetAssociatedPartMaker), context => Variants.Value(PartUtils.GetAssociatedPartMaker(context, part.Id)));
        manager.Register(nameof(PartUtils.GetSplittingCurves), context => Variants.Value(PartUtils.GetSplittingCurves(context, part.Id)));
        manager.Register(nameof(PartUtils.GetSplittingElements), context => Variants.Value(PartUtils.GetSplittingElements(context, part.Id)));
        manager.Register(nameof(PartUtils.HasAssociatedParts), context => Variants.Value(PartUtils.HasAssociatedParts(context, part.Id)));
    }
}