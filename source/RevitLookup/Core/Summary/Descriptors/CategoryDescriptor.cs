﻿// Copyright 2003-2024 by Autodesk, Inc.
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

namespace RevitLookup.Core.Summary.Descriptors;

public sealed class CategoryDescriptor : Descriptor, IDescriptorResolver, IDescriptorExtension, IDescriptorExtension<Document>
{
    private readonly Category _category;

    public CategoryDescriptor(Category category)
    {
        _category = category;
        Name = category.Name;
    }

    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            "AllowsVisibilityControl" => ResolveAllowsVisibilityControl,
            "Visible" => ResolveVisible,
            nameof(Category.GetGraphicsStyle) => ResolveGetGraphicsStyle,
            nameof(Category.GetLinePatternId) => ResolveGetLinePatternId,
            nameof(Category.GetLineWeight) => ResolveGetLineWeight,
            _ => null
        };

        IVariant ResolveGetLineWeight()
        {
            return Variants.Values<int?>(2)
                .Add(_category.GetLineWeight(GraphicsStyleType.Cut), "Cut")
                .Add(_category.GetLineWeight(GraphicsStyleType.Projection), "Projection")
                .Consume();
        }

        IVariant ResolveGetLinePatternId()
        {
            return Variants.Values<ElementId>(2)
                .Add(_category.GetLinePatternId(GraphicsStyleType.Cut), "Cut")
                .Add(_category.GetLinePatternId(GraphicsStyleType.Projection), "Projection")
                .Consume();
        }

        IVariant ResolveGetGraphicsStyle()
        {
            return Variants.Values<GraphicsStyle>(2)
                .Add(_category.GetGraphicsStyle(GraphicsStyleType.Cut), "Cut")
                .Add(_category.GetGraphicsStyle(GraphicsStyleType.Projection), "Projection")
                .Consume();
        }

        IVariant ResolveAllowsVisibilityControl()
        {
            return Variants.Value(_category.get_AllowsVisibilityControl(Context.ActiveView), "Active view");
        }

        IVariant ResolveVisible()
        {
            return Variants.Value(_category.get_Visible(Context.ActiveView), "Active view");
        }
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
#if !REVIT2023_OR_GREATER
        manager.Register("BuiltInCategory", () => Variants.Value((BuiltInCategory) _category.Id.IntegerValue));
#endif
    }

    public void RegisterExtensions(IExtensionManager<Document> manager)
    {
        manager.Register("GetElements", context =>
        {
            return Variants.Value(context
#if REVIT2023_OR_GREATER
                .GetInstances(_category.BuiltInCategory));
#else
                .GetInstances((BuiltInCategory) _category.Id.IntegerValue));
#endif
        });
    }
}