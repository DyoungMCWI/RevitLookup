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

public sealed class ScheduleDefinitionDescriptor(ScheduleDefinition scheduleDefinition) : Descriptor, IDescriptorResolver, IDescriptorResolver<Document>
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(ScheduleDefinition.CanFilterByGlobalParameters) => ResolveFilterByGlobalParameters,
            nameof(ScheduleDefinition.CanFilterByParameterExistence) => ResolveFilterByParameterExistence,
            nameof(ScheduleDefinition.CanFilterBySubstring) => ResolveFilterBySubstring,
            nameof(ScheduleDefinition.CanFilterByValue) => ResolveFilterByValue,
            nameof(ScheduleDefinition.CanFilterByValuePresence) => ResolveFilterByValuePresence,
            nameof(ScheduleDefinition.CanSortByField) => ResolveSortByField,
            nameof(ScheduleDefinition.GetField) => ResolveGetField,
            nameof(ScheduleDefinition.GetFieldId) => ResolveGetFieldId,
            nameof(ScheduleDefinition.GetFieldIndex) => ResolveGetFieldIndex,
            nameof(ScheduleDefinition.GetFilter) => ResolveGetFilter,
            nameof(ScheduleDefinition.GetSortGroupField) => ResolveGetSortGroupField,
            _ => null
        };

        IVariant ResolveFilterByGlobalParameters()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = Variants.Values<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByGlobalParameters(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveFilterByParameterExistence()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = Variants.Values<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByParameterExistence(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveFilterBySubstring()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = Variants.Values<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterBySubstring(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveFilterByValue()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = Variants.Values<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByValue(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveFilterByValuePresence()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = Variants.Values<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanFilterByValuePresence(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveSortByField()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = Variants.Values<bool>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.CanSortByField(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetField()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = Variants.Values<ScheduleField>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.GetField(field);
                variants.Add(result, $"{result.GetName()}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetFieldId()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = Variants.Values<ScheduleFieldId>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.GetFieldId(field.IntegerValue);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetFieldIndex()
        {
            var fields = scheduleDefinition.GetFieldOrder();
            var variants = Variants.Values<int>(fields.Count);
            foreach (var field in fields)
            {
                var result = scheduleDefinition.GetFieldIndex(field);
                var name = scheduleDefinition.GetField(field).GetName();
                variants.Add(result, $"{name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetFilter()
        {
            var count = scheduleDefinition.GetFilterCount();
            var variants = Variants.Values<ScheduleFilter>(count);
            for (var i = 0; i < count; i++)
            {
                variants.Add(scheduleDefinition.GetFilter(i));
            }

            return variants.Consume();
        }

        IVariant ResolveGetSortGroupField()
        {
            var count = scheduleDefinition.GetSortGroupFieldCount();
            var variants = Variants.Values<ScheduleSortGroupField>(count);
            for (var i = 0; i < count; i++)
            {
                variants.Add(scheduleDefinition.GetSortGroupField(i));
            }

            return variants.Consume();
        }
    }

    Func<Document, IVariant>? IDescriptorResolver<Document>.Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(ScheduleDefinition.IsValidCategoryForEmbeddedSchedule) => ResolveValidCategoryForEmbeddedSchedule,
            _ => null
        };

        IVariant ResolveValidCategoryForEmbeddedSchedule(Document context)
        {
            var categories = context.Settings.Categories;
            var variants = Variants.Values<bool>(categories.Size);
            foreach (Category category in categories)
            {
                if (scheduleDefinition.IsValidCategoryForEmbeddedSchedule(category.Id))
                {
                    variants.Add(true, category.Name);
                }
            }

            return variants.Consume();
        }
    }
}