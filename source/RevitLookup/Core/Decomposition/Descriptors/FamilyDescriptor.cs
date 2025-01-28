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

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class FamilyDescriptor(Family family) : ElementDescriptor(family)
{
    public override Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return null;
    }

    public override void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(FamilySizeTableManager.GetFamilySizeTableManager), RegisterGetFamilySizeTableManager);
        manager.Register(nameof(FamilyUtils.FamilyCanConvertToFaceHostBased), RegisterFamilyCanConvertToFaceHostBased);
        manager.Register(nameof(FamilyUtils.GetProfileSymbols), RegisterProfileSymbols);
        return;

        IVariant RegisterGetFamilySizeTableManager()
        {
            return Variants.Value(FamilySizeTableManager.GetFamilySizeTableManager(family.Document, family.Id));
        }

        IVariant RegisterFamilyCanConvertToFaceHostBased()
        {
            return Variants.Value(FamilyUtils.FamilyCanConvertToFaceHostBased(family.Document, family.Id));
        }

        IVariant RegisterProfileSymbols()
        {
            var values = Enum.GetValues(typeof(ProfileFamilyUsage));
            var capacity = values.Length * 2;
            var variants = Variants.Values<ICollection<ElementId>>(capacity);

            foreach (ProfileFamilyUsage value in values)
            {
                variants.Add(FamilyUtils.GetProfileSymbols(family.Document, value, false), $"{value}, with multiple curve loops");
                variants.Add(FamilyUtils.GetProfileSymbols(family.Document, value, true), $"{value}, with single curve loop");
            }

            return variants.Consume();
        }
    }
}