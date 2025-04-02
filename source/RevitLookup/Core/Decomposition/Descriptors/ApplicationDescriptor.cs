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

using Autodesk.Revit.DB.Macros;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class ApplicationDescriptor : Descriptor, IDescriptorExtension
{
    private readonly Autodesk.Revit.ApplicationServices.Application _application;

    public ApplicationDescriptor(Autodesk.Revit.ApplicationServices.Application application)
    {
        _application = application;
        Name = application.VersionName;
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("GetFormulaFunctions", () => Variants.Value(FormulaManager.GetFunctions()));
        manager.Register("GetFormulaOperators", () => Variants.Value(FormulaManager.GetOperators()));
        manager.Register(nameof(MacroManager.GetMacroManager), () => Variants.Value(MacroManager.GetMacroManager(_application)));
    }
}