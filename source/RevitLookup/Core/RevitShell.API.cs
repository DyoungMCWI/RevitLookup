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
using System.Runtime.InteropServices;

namespace RevitLookup.Core;

public static partial class RevitShell
{
    public static Parameter GetBuiltinParameter(BuiltInParameter builtInParameter)
    {
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var documentType = typeof(Document);
        var parameterType = typeof(Parameter);
        var assembly = Assembly.GetAssembly(parameterType)!;
        var aDocumentType = assembly.GetType("ADocument")!;
        var elementIdType = assembly.GetType("ElementId")!;
        var elementIdIdType = elementIdType.GetField("<alignment member>", bindingFlags)!;
        var getADocumentType = documentType.GetMethod("getADocument", bindingFlags)!;
        var parameterCtorType = parameterType.GetConstructor(bindingFlags, null, [aDocumentType.MakePointerType(), elementIdType.MakePointerType()], null)!;

        var elementId = Activator.CreateInstance(elementIdType)!;
        elementIdIdType.SetValue(elementId, builtInParameter);

        var handle = GCHandle.Alloc(elementId);
        var elementIdPointer = GCHandle.ToIntPtr(handle);
        Marshal.StructureToPtr(elementId, elementIdPointer, true);

        var parameter = (Parameter) parameterCtorType.Invoke([getADocumentType.Invoke(Context.ActiveDocument, null), elementIdPointer]);
        handle.Free();

        return parameter;
    }

    public static Category GetBuiltinCategory(BuiltInCategory builtInCategory)
    {
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var documentType = typeof(Document);
        var categoryType = typeof(Category);
        var assembly = Assembly.GetAssembly(categoryType)!;
        var aDocumentType = assembly.GetType("ADocument")!;
        var elementIdType = assembly.GetType("ElementId")!;
        var elementIdIdType = elementIdType.GetField("<alignment member>", bindingFlags)!;
        var getADocumentType = documentType.GetMethod("getADocument", bindingFlags)!;
        var categoryCtorType = categoryType.GetConstructor(bindingFlags, null, [aDocumentType.MakePointerType(), elementIdType.MakePointerType()], null)!;

        var elementId = Activator.CreateInstance(elementIdType)!;
        elementIdIdType.SetValue(elementId, builtInCategory);

        var handle = GCHandle.Alloc(elementId);
        var elementIdPointer = GCHandle.ToIntPtr(handle);
        Marshal.StructureToPtr(elementId, elementIdPointer, true);

        var category = (Category) categoryCtorType.Invoke([getADocumentType.Invoke(Context.ActiveDocument, null), elementIdPointer]);
        handle.Free();

        return category;
    }

    public static string GetParameterValue(Parameter parameter)
    {
        return parameter.StorageType switch
        {
            StorageType.Integer => parameter.AsInteger().ToString(),
            StorageType.Double => parameter.AsValueString(),
            StorageType.String => parameter.AsString(),
            StorageType.ElementId => parameter.AsElementId().ToString(),
            StorageType.None => parameter.AsValueString(),
            _ => parameter.AsValueString()
        };
    }

    public static void UpdateParameterValue(Parameter parameter, string value)
    {
        var transaction = new Transaction(parameter.Element.Document);
        transaction.Start("Set parameter value");

        bool result;
        switch (parameter.StorageType)
        {
            case StorageType.Integer:
                result = int.TryParse(value, out var intValue);
                if (!result) break;

                result = parameter.Set(intValue);
                break;
            case StorageType.Double:
                result = parameter.SetValueString(value);
                break;
            case StorageType.String:
                result = parameter.Set(value);
                break;
            case StorageType.ElementId:
#if REVIT2024_OR_GREATER
                result = long.TryParse(value, out var idValue);
#else
                result = int.TryParse(value, out var idValue);
#endif
                if (!result) break;

                result = parameter.Set(new ElementId(idValue));
                break;
            case StorageType.None:
            default:
                result = parameter.SetValueString(value);
                break;
        }

        if (result)
        {
            transaction.Commit();
        }
        else
        {
            transaction.RollBack();
            throw new ArgumentException("Invalid parameter value");
        }
    }
}