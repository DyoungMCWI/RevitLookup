﻿using System.Reflection;
using System.Text;
using RevitLookup.Abstractions.Models.Tools;

namespace RevitLookup.Core.Tools.Units;

public static class UnitsCollector
{
    public static List<UnitInfo> GetBuiltinParametersInfo()
    {
        var parameters = Enum.GetValues(typeof(BuiltInParameter));
        var result = new List<UnitInfo>(parameters.Length);
        foreach (BuiltInParameter parameter in parameters)
            try
            {
                result.Add(new UnitInfo
                {
                    Unit = parameter.ToString(),
                    Label = parameter.ToLabel(),
                    Value = parameter
                });
            }
            catch
            {
                // ignored
                // Some parameters don't have a label
            }

        return result;
    }

    public static List<UnitInfo> GetBuiltinCategoriesInfo()
    {
        var categories = Enum.GetValues(typeof(BuiltInCategory));
        var result = new List<UnitInfo>(categories.Length);
        foreach (BuiltInCategory category in categories)
            try
            {
                result.Add(new UnitInfo
                {
                    Unit = category.ToString(),
                    Label = category.ToLabel(),
                    Value = category
                });
            }
            catch
            {
                // ignored
                // Some categories don't have a label
            }

        return result;
    }

    public static List<UnitInfo> GetForgeInfo()
    {
        const BindingFlags searchFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly;

        var dataTypes = new List<PropertyInfo>();
#if REVIT2022_OR_GREATER
        dataTypes.AddRange(typeof(UnitTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId.Boolean).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId.String).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId.Int).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId.Reference).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(ParameterTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(GroupTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(DisciplineTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SymbolTypeId).GetProperties(searchFlags));
#else
        dataTypes.AddRange(typeof(UnitTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SpecTypeId).GetProperties(searchFlags));
        dataTypes.AddRange(typeof(SymbolTypeId).GetProperties(searchFlags));
#endif
        return dataTypes.Select(info =>
            {
                var typeId = (ForgeTypeId) info.GetValue(null)!;
                return new UnitInfo
                {
                    Unit = typeId.TypeId,
                    Label = GetLabel(typeId, info),
                    Value = GetClassName(info)
                };
            })
            .ToList();
    }


    private static string GetClassName(PropertyInfo property)
    {
        var type = property.DeclaringType!;
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(type.Name);
        stringBuilder.Append('.');
        stringBuilder.Append(property.Name);

        while (type.IsNested)
        {
            type = type.DeclaringType!;
            stringBuilder.Insert(0, '.');
            stringBuilder.Insert(0, type.Name);
        }

        return stringBuilder.ToString();
    }

    private static string GetLabel(ForgeTypeId typeId, PropertyInfo property)
    {
        if (typeId.Empty()) return string.Empty;
        if (property.Name == nameof(SpecTypeId.Custom)) return string.Empty;

        var type = property.DeclaringType;
        while (type!.IsNested)
        {
            type = type.DeclaringType;
        }

        try
        {
            return type.Name switch
            {
                nameof(UnitTypeId) => typeId.ToUnitLabel(),
                nameof(SpecTypeId) => typeId.ToSpecLabel(),
                nameof(SymbolTypeId) => typeId.ToSymbolLabel(),
#if REVIT2022_OR_GREATER
                nameof(ParameterTypeId) => typeId.ToParameterLabel(),
                nameof(GroupTypeId) => typeId.ToGroupLabel(),
                nameof(DisciplineTypeId) => typeId.ToDisciplineLabel(),
#endif
                _ => throw new ArgumentOutOfRangeException(nameof(typeId), typeId, "Unknown Forge Type Identifier")
            };
        }
        catch
        {
            //Some parameter label thrown an exception
            return string.Empty;
        }
    }
}