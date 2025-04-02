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

public sealed class FamilySizeTableDescriptor(FamilySizeTable table) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(FamilySizeTable.GetColumnHeader) => ResolveColumnHeader,
            nameof(FamilySizeTable.IsValidColumnIndex) => ResolveIsValidColumnIndex,
            _ => null
        };

        IVariant ResolveColumnHeader()
        {
            var count = table.NumberOfColumns;
            var variants = Variants.Values<FamilySizeTableColumn>(count);

            for (var i = 0; i < count; i++)
            {
                variants.Add(table.GetColumnHeader(i));
            }

            return variants.Consume();
        }

        IVariant ResolveIsValidColumnIndex()
        {
            var count = table.NumberOfColumns;
            var variants = Variants.Values<bool>(count);

            for (var i = 0; i <= count; i++)
            {
                var result = table.IsValidColumnIndex(i);
                variants.Add(result, $"{i}: {result}");
            }

            return variants.Consume();
        }
    }

    // TODO: rework FamilySizeTableEditDialog
    // public void RegisterMenu(ContextMenu contextMenu)
    // {
    //     var context = (ISnoopViewModel) contextMenu.DataContext;
    //     var document = context.SnoopableObjects[0].Context;
    //     
    //     contextMenu.AddMenuItem("ShowMenuItem")
    //         .SetHeader("Show table")
    //         .SetAvailability(table.IsValidObject)
    //         .SetCommand(table, async sizeTable =>
    //         {
    //             try
    //             {
    //                 var dialog = new FamilySizeTableEditDialog(document, sizeTable);
    //                 await dialog.ShowAsync();
    //             }
    //             catch (Exception exception)
    //             {
    //                 var logger = context.ServiceProvider.GetRequiredService<ILogger<FamilySizeTableDescriptor>>();
    //                 logger.LogError(exception, "FamilySizeTableDialog error");
    //             }
    //         });
    // }
}