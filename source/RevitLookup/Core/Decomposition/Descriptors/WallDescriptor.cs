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

public class WallDescriptor(Wall wall) : ElementDescriptor(wall)
{
    public override Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
#if REVIT2022_OR_GREATER
            nameof(Wall.IsWallCrossSectionValid) => ResolveIsWallCrossSectionValid,
#endif
            _ => null
        };
#if REVIT2022_OR_GREATER
        IVariant ResolveIsWallCrossSectionValid()
        {
            var values = Enum.GetValues(typeof(WallCrossSection));
            var variants = Variants.Values<bool>(values.Length);

            foreach (WallCrossSection crossSection in values)
            {
                var result = wall.IsWallCrossSectionValid(crossSection);
                variants.Add(result, $"{crossSection}: {result}");
            }

            return variants.Consume();
        }
#endif
    }

    public override void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(WallUtils.IsWallJoinAllowedAtEnd), ResolveIsWallJoinAllowedAtEnd);
    }

    private IVariant ResolveIsWallJoinAllowedAtEnd()
    {
        var variants = Variants.Values<bool>(2);
        var startResult = WallUtils.IsWallJoinAllowedAtEnd(wall, 0);
        var endResult = WallUtils.IsWallJoinAllowedAtEnd(wall, 1);
        variants.Add(startResult, $"Start: {startResult}");
        variants.Add(endResult, $"End: {endResult}");

        return variants.Consume();
    }
}