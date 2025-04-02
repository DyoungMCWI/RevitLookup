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

using System.Globalization;
using System.Reflection;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class TableSectionDataDescriptor(TableSectionData tableSectionData) : Descriptor, IDescriptorResolver, IDescriptorResolver<Document>
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(TableSectionData.AllowOverrideCellStyle) => ResolveOverrideCellStyle,
            nameof(TableSectionData.CanInsertColumn) => ResolveCanInsertColumn,
            nameof(TableSectionData.CanInsertRow) => ResolveCanInsertRow,
            nameof(TableSectionData.CanRemoveColumn) => ResolveCanRemoveColumn,
            nameof(TableSectionData.CanRemoveRow) => ResolveCanRemoveRow,
            nameof(TableSectionData.GetCellCalculatedValue) when parameters.Length == 1 => ResolveCellCalculatedValueForColumns,
            nameof(TableSectionData.GetCellCalculatedValue) when parameters.Length == 2 => ResolveCellCalculatedValueForTable,
            nameof(TableSectionData.GetCellCombinedParameters) when parameters.Length == 1 => ResolveCellCombinedParametersForColumns,
            nameof(TableSectionData.GetCellCombinedParameters) when parameters.Length == 2 => ResolveCellCombinedParametersForTable,
            nameof(TableSectionData.GetCellSpec) => ResolveCellSpec,
            nameof(TableSectionData.GetCellText) => ResolveCellText,
            nameof(TableSectionData.GetCellType) when parameters.Length == 1 => ResolveCellTypeForColumns,
            nameof(TableSectionData.GetCellType) when parameters.Length == 2 => ResolveCellTypeForTable,
            nameof(TableSectionData.GetColumnWidth) => ResolveColumnWidth,
            nameof(TableSectionData.GetColumnWidthInPixels) => ResolveColumnWidthInPixels,
            nameof(TableSectionData.GetMergedCell) => ResolveMergedCell,
            nameof(TableSectionData.GetRowHeight) => ResolveRowHeight,
            nameof(TableSectionData.GetRowHeightInPixels) => ResolveRowHeightInPixels,
            nameof(TableSectionData.GetTableCellStyle) => ResolveTableCellStyle,
            nameof(TableSectionData.IsCellFormattable) => ResolveIsCellFormattable,
            nameof(TableSectionData.IsCellOverridden) when parameters.Length == 1 => ResolveIsCellOverriddenForColumns,
            nameof(TableSectionData.IsCellOverridden) when parameters.Length == 2 => ResolveIsCellOverriddenForTable,
            nameof(TableSectionData.IsValidColumnNumber) => ResolveIsValidColumnNumber,
            nameof(TableSectionData.IsValidRowNumber) => ResolveIsValidRowNumber,
            nameof(TableSectionData.RefreshData) => Variants.Disabled,
            _ => null
        };

        IVariant ResolveOverrideCellStyle()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<bool>(rowsNumber * columnsNumber);
            for (var i = 0; i < rowsNumber; i++)
            {
                for (var j = 0; j < columnsNumber; j++)
                {
                    var result = tableSectionData.AllowOverrideCellStyle(i, j);
                    variants.Add(result, $"Row {i}, Column {j}: {result}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveCanInsertColumn()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanInsertColumn(i);
                variants.Add(result, $"{i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveCanInsertRow()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = Variants.Values<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanInsertRow(i);
                variants.Add(result, $"{i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveCanRemoveColumn()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanRemoveColumn(i);
                variants.Add(result, $"{i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveCanRemoveRow()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = Variants.Values<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.CanRemoveRow(i);
                variants.Add(result, $"{i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveCellCalculatedValueForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<TableCellCalculatedValueData>(columnsNumber);
            for (var j = 0; j < columnsNumber; j++)
            {
                var result = tableSectionData.GetCellCalculatedValue(j);
                variants.Add(result, $"Column {j}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveCellCalculatedValueForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<TableCellCalculatedValueData>(rowsNumber * columnsNumber);
            for (var i = 0; i < rowsNumber; i++)
            {
                for (var j = 0; j < columnsNumber; j++)
                {
                    var result = tableSectionData.GetCellCalculatedValue(i, j);
                    variants.Add(result, $"Row {i}, Column {j}: {result}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveCellCombinedParametersForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<IList<TableCellCombinedParameterData>>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellCombinedParameters(i);
                variants.Add(result, $"Column {i}");
            }

            return variants.Consume();
        }

        IVariant ResolveCellCombinedParametersForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<IList<TableCellCombinedParameterData>>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellCombinedParameters(j, i);
                    variants.Add(result, $"Row {j}, Column {i}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveCellSpec()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<ForgeTypeId>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellSpec(j, i);
                    if (result.Empty()) continue;

                    variants.Add(result, $"Row {j}, Column {i}: {result.ToSpecLabel()}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveCellText()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<string>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellText(j, i);
                    variants.Add(result, $"Row {j}, Column {i}: {result}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveCellTypeForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<CellType>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellType(i);
                variants.Add(result, $"Column {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveCellTypeForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<CellType>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellType(j, i);
                    variants.Add(result, $"Row {j}, Column {i}: {result}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveColumnWidth()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<double>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetColumnWidth(i);
                variants.Add(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }

            return variants.Consume();
        }


        IVariant ResolveColumnWidthInPixels()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<int>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetColumnWidthInPixels(i);
                variants.Add(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }

            return variants.Consume();
        }

        IVariant ResolveMergedCell()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<TableMergedCell>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetMergedCell(j, i);
                    variants.Add(result, $"Row {j}, Column {i}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveRowHeight()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = Variants.Values<double>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetRowHeight(i);
                variants.Add(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }

            return variants.Consume();
        }

        IVariant ResolveRowHeightInPixels()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = Variants.Values<int>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.GetRowHeightInPixels(i);
                variants.Add(result, $"{i}: {result.ToString(CultureInfo.InvariantCulture)}");
            }

            return variants.Consume();
        }

        IVariant ResolveTableCellStyle()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<TableCellStyle>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetTableCellStyle(j, i);
                    variants.Add(result, $"Row {j}, Column {i}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveIsCellFormattable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<bool>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.IsCellFormattable(j, i);
                    variants.Add(result, $"Row {j}, Column {i}: {result}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveIsCellOverriddenForColumns()
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<bool>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.IsCellOverridden(i);
                variants.Add(result, $"Column {i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveIsCellOverriddenForTable()
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<bool>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.IsCellOverridden(j, i);
                    variants.Add(result, $"Row {j}, Column {i}: {result}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveIsValidColumnNumber()
        {
            var count = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.IsValidColumnNumber(i);
                variants.Add(result, $"{i}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveIsValidRowNumber()
        {
            var count = tableSectionData.NumberOfRows;
            var variants = Variants.Values<bool>(count);
            for (var i = 0; i < count; i++)
            {
                var result = tableSectionData.IsValidRowNumber(i);
                variants.Add(result, $"{i}: {result}");
            }

            return variants.Consume();
        }
    }

    Func<Document, IVariant>? IDescriptorResolver<Document>.Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(TableSectionData.GetCellCategoryId) when parameters.Length == 1 => ResolveCellCategoryIdForColumns,
            nameof(TableSectionData.GetCellCategoryId) when parameters.Length == 2 => ResolveCellCategoryIdForTable,
            nameof(TableSectionData.GetCellFormatOptions) when parameters.Length == 2 => ResolveCellFormatOptionsForColumns,
            nameof(TableSectionData.GetCellFormatOptions) when parameters.Length == 3 => ResolveCellFormatOptionsForTable,
            nameof(TableSectionData.GetCellParamId) when parameters.Length == 1 => ResolveCellParamIdForColumns,
            nameof(TableSectionData.GetCellParamId) when parameters.Length == 2 => ResolveCellParamIdForTable,
            _ => null
        };

        IVariant ResolveCellCategoryIdForColumns(Document context)
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<ElementId>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellCategoryId(i);
                if (result == ElementId.InvalidElementId) continue;

                var category = Category.GetCategory(context, result);
                if (category is null) continue;

                variants.Add(result, $"Column {i}: {category.Name}");
            }

            return variants.Consume();
        }

        IVariant ResolveCellCategoryIdForTable(Document context)
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<ElementId>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellCategoryId(j, i);
                    if (result == ElementId.InvalidElementId) continue;

                    var category = Category.GetCategory(context, result);
                    if (category is null) continue;

                    variants.Add(result, $"Row {j}, Column {i}: {category.Name}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveCellFormatOptionsForColumns(Document context)
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<FormatOptions>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellFormatOptions(i, context);
                variants.Add(result, $"Column {i}");
            }

            return variants.Consume();
        }

        IVariant ResolveCellFormatOptionsForTable(Document context)
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<FormatOptions>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellFormatOptions(j, i, context);
                    variants.Add(result, $"Row {j}, Column {i}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveCellParamIdForColumns(Document context)
        {
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<ElementId>(columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                var result = tableSectionData.GetCellParamId(i);
                if (result != ElementId.InvalidElementId)
                {
                    var parameter = result.ToElement(context);
                    variants.Add(result, $"Column {i}: {parameter!.Name}");
                }
            }

            return variants.Consume();
        }

        IVariant ResolveCellParamIdForTable(Document context)
        {
            var rowsNumber = tableSectionData.NumberOfRows;
            var columnsNumber = tableSectionData.NumberOfColumns;
            var variants = Variants.Values<ElementId>(rowsNumber * columnsNumber);
            for (var i = 0; i < columnsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    var result = tableSectionData.GetCellParamId(j, i);
                    if (result == ElementId.InvalidElementId) continue;

                    var parameter = result.ToElement(context);
                    if (parameter is null) continue;

                    variants.Add(result, $"Row {j}, Column {i}: {parameter.Name}");
                }
            }

            return variants.Consume();
        }
    }
}