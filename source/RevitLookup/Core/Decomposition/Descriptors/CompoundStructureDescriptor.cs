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

public class CompoundStructureDescriptor(CompoundStructure compoundStructure) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(CompoundStructure.CanLayerBeStructuralMaterial) => ResolveCanLayerBeStructuralMaterial,
            nameof(CompoundStructure.CanLayerBeVariable) => ResolveCanLayerBeVariable,
            nameof(CompoundStructure.CanLayerWidthBeNonZero) => ResolveCanLayerWidthBeNonZero,
            nameof(CompoundStructure.GetAdjacentRegions) => ResolveGetAdjacentRegions,
            nameof(CompoundStructure.GetCoreBoundaryLayerIndex) => ResolveGetCoreBoundaryLayerIndex,
            nameof(CompoundStructure.GetDeckEmbeddingType) => ResolveGetDeckEmbeddingType,
            nameof(CompoundStructure.GetDeckProfileId) => ResolveGetDeckProfileId,
            nameof(CompoundStructure.GetLayerAssociatedToRegion) => ResolveGetLayerAssociatedToRegion,
            nameof(CompoundStructure.GetLayerFunction) => ResolveGetLayerFunction,
            nameof(CompoundStructure.GetLayerWidth) => ResolveGetLayerWidth,
            nameof(CompoundStructure.GetMaterialId) => ResolveGetMaterialId,
            nameof(CompoundStructure.GetNumberOfShellLayers) => ResolveGetNumberOfShellLayers,
            nameof(CompoundStructure.GetOffsetForLocationLine) => ResolveGetOffsetForLocationLine,
            nameof(CompoundStructure.GetPreviousNonZeroLayerIndex) => ResolveGetPreviousNonZeroLayerIndex,
            nameof(CompoundStructure.GetRegionEnvelope) => ResolveGetRegionEnvelope,
            nameof(CompoundStructure.GetRegionsAssociatedToLayer) => ResolveGetRegionsAssociatedToLayer,
            nameof(CompoundStructure.GetSegmentCoordinate) => ResolveGetSegmentCoordinate,
            nameof(CompoundStructure.GetSegmentOrientation) => ResolveGetSegmentOrientation,
            nameof(CompoundStructure.GetWallSweepsInfo) => ResolveGetWallSweepsInfo,
            nameof(CompoundStructure.GetWidth) when parameters.Length == 1 => ResolveGetWidth,
            nameof(CompoundStructure.IsCoreLayer) => ResolveIsCoreLayer,
            nameof(CompoundStructure.IsRectangularRegion) => ResolveIsRectangularRegion,
            nameof(CompoundStructure.IsSimpleRegion) => ResolveIsSimpleRegion,
            nameof(CompoundStructure.IsStructuralDeck) => ResolveIsStructuralDeck,
            nameof(CompoundStructure.ParticipatesInWrapping) => ResolveParticipatesInWrapping,
            _ => null
        };

        IVariant ResolveCanLayerBeStructuralMaterial()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<bool>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.CanLayerBeStructuralMaterial(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveCanLayerBeVariable()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<bool>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.CanLayerBeVariable(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveCanLayerWidthBeNonZero()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<bool>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.CanLayerWidthBeNonZero(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetAdjacentRegions()
        {
            var regionsCount = compoundStructure.GetRegionIds().Count;
            var variants = Variants.Values<IList<int>>(regionsCount);

            for (var i = 0; i < regionsCount; i++)
            {
                var result = compoundStructure.GetAdjacentRegions(i);
                variants.Add(result, $"Region" + $" {i}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetCoreBoundaryLayerIndex()
        {
            var values = Enum.GetValues(typeof(ShellLayerType));
            var variants = Variants.Values<int>(values.Length);

            foreach (ShellLayerType value in values)
            {
                var result = compoundStructure.GetCoreBoundaryLayerIndex(value);
                variants.Add(result, $"{value.ToString()}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetDeckEmbeddingType()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<StructDeckEmbeddingType>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetDeckEmbeddingType(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetLayerAssociatedToRegion()
        {
            var regionsCount = compoundStructure.GetRegionIds().Count;
            var variants = Variants.Values<int>(regionsCount);

            for (var i = 0; i < regionsCount; i++)
            {
                var result = compoundStructure.GetLayerAssociatedToRegion(i);
                variants.Add(result, $"Region {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetLayerFunction()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<MaterialFunctionAssignment>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetLayerFunction(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetDeckProfileId()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<ElementId>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetDeckProfileId(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetLayerWidth()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<double>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetLayerWidth(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetMaterialId()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<ElementId>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetMaterialId(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetNumberOfShellLayers()
        {
            var values = Enum.GetValues(typeof(ShellLayerType));
            var variants = Variants.Values<int>(values.Length);

            foreach (ShellLayerType value in values)
            {
                var result = compoundStructure.GetNumberOfShellLayers(value);
                variants.Add(result, $"{value.ToString()}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetOffsetForLocationLine()
        {
            var values = Enum.GetValues(typeof(WallLocationLine));
            var variants = Variants.Values<double>(values.Length);

            foreach (WallLocationLine value in values)
            {
                var result = compoundStructure.GetOffsetForLocationLine(value);
                variants.Add(result, $"{value.ToString()}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetPreviousNonZeroLayerIndex()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<int>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetPreviousNonZeroLayerIndex(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetRegionEnvelope()
        {
            var regionsCount = compoundStructure.GetRegionIds().Count;
            var variants = Variants.Values<BoundingBoxUV>(regionsCount);

            for (var i = 0; i < regionsCount; i++)
            {
                var result = compoundStructure.GetRegionEnvelope(i);
                variants.Add(result, $"Region {i}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetRegionsAssociatedToLayer()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<IList<int>>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetRegionsAssociatedToLayer(i);
                variants.Add(result, $"Layer {i}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetSegmentCoordinate()
        {
            var segmentCount = compoundStructure.GetSegmentIds().Count;
            var variants = Variants.Values<double>(segmentCount);
            for (var i = 0; i < segmentCount; i++)
            {
                var result = compoundStructure.GetSegmentCoordinate(i);
                variants.Add(result, $"Segment {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetSegmentOrientation()
        {
            var segmentCount = compoundStructure.GetSegmentIds().Count;
            var variants = Variants.Values<RectangularGridSegmentOrientation>(segmentCount);
            for (var i = 0; i < segmentCount; i++)
            {
                var result = compoundStructure.GetSegmentOrientation(i);
                variants.Add(result, $"Segment {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetWallSweepsInfo()
        {
            var values = Enum.GetValues(typeof(WallSweepType));
            var variants = Variants.Values<IList<WallSweepInfo>>(values.Length);

            foreach (WallSweepType value in values)
            {
                var result = compoundStructure.GetWallSweepsInfo(value);
                variants.Add(result, value.ToString());
            }

            return variants.Consume();
        }

        IVariant ResolveGetWidth()
        {
            var regionsCount = compoundStructure.GetRegionIds().Count;
            var variants = Variants.Values<double>(regionsCount);

            for (var i = 0; i < regionsCount; i++)
            {
                var result = compoundStructure.GetWidth(i);
                variants.Add(result, $"Region {i}: {result}");
            }

            return variants.Consume();
        }
        
        IVariant ResolveIsCoreLayer()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<bool>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.IsCoreLayer(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }
        
        IVariant ResolveIsRectangularRegion()
        {
            var regionsCount = compoundStructure.GetRegionIds().Count;
            var variants = Variants.Values<bool>(regionsCount);

            for (var i = 0; i < regionsCount; i++)
            {
                var result = compoundStructure.IsRectangularRegion(i);
                variants.Add(result, $"Region {i}: {result}");
            }

            return variants.Consume();
        }
        
        IVariant ResolveIsSimpleRegion()
        {
            var regionsCount = compoundStructure.GetRegionIds().Count;
            var variants = Variants.Values<bool>(regionsCount);

            for (var i = 0; i < regionsCount; i++)
            {
                var result = compoundStructure.IsSimpleRegion(i);
                variants.Add(result, $"Region {i}: {result}");
            }

            return variants.Consume();
        }
        
        IVariant ResolveIsStructuralDeck()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<bool>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.IsStructuralDeck(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }
        
        IVariant ResolveParticipatesInWrapping()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = Variants.Values<bool>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.ParticipatesInWrapping(i);
                variants.Add(result, $"Layer {i}: {result}");
            }

            return variants.Consume();
        }
    }
}