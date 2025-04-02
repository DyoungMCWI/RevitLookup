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

using System.Collections;
using System.Reflection;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Macros;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class EnumerableDescriptor : Descriptor, IDescriptorEnumerator, IDescriptorResolver
{
    public EnumerableDescriptor(IEnumerable value)
    {
        // ReSharper disable once GenericEnumeratorNotDisposed
        Enumerator = value.GetEnumerator();

        //Checking types to reduce memory allocation when creating an iterator and increase performance
        IsEmpty = value switch
        {
            string => true,
            ICollection enumerable => enumerable.Count == 0,
            ParameterSet enumerable => enumerable.IsEmpty,
            ParameterMap enumerable => enumerable.IsEmpty,
            DefinitionBindingMap enumerable => enumerable.IsEmpty,
            CategoryNameMap enumerable => enumerable.IsEmpty,
            DefinitionGroups enumerable => enumerable.IsEmpty,
            HashSet<ElementId> enumerable => enumerable.Count == 0,
            HashSet<ElectricalSystem> enumerable => enumerable.Count == 0,
            DocumentSet enumerable => enumerable.IsEmpty,
            PhaseArray enumerable => enumerable.IsEmpty,
            ProjectLocationSet enumerable => enumerable.IsEmpty,
            PlanTopologySet enumerable => enumerable.IsEmpty,
            CitySet enumerable => enumerable.IsEmpty,
            WireTypeSet enumerable => enumerable.IsEmpty,
            PanelTypeSet enumerable => enumerable.IsEmpty,
            FamilyTypeSet enumerable => enumerable.IsEmpty,
            MullionTypeSet enumerable => enumerable.IsEmpty,
            VoltageTypeSet enumerable => enumerable.IsEmpty,
            InsulationTypeSet enumerable => enumerable.IsEmpty,
            TemperatureRatingTypeSet enumerable => enumerable.IsEmpty,
            MacroManager enumerable => enumerable.Count == 0,
            _ => !Enumerator.MoveNext()
        };
    }

    public IEnumerator Enumerator { get; }
    public bool IsEmpty { get; }

    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(IEnumerable.GetEnumerator) => ResolveGetEnumerator,
            _ => null
        };

        IVariant ResolveGetEnumerator()
        {
            return Variants.Empty<IEnumerator>();
        }
    }
}