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

using System.Collections;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI.Selection;
using Autodesk.Windows;
using RevitLookup.Abstractions.Models.Decomposition;

namespace RevitLookup.Core.Decomposition;

public static class RevitObjectsCollector
{
    public static IEnumerable GetObjects(KnownDecompositionObject decompositionObject)
    {
        return decompositionObject switch
        {
            KnownDecompositionObject.View => FindView(),
            KnownDecompositionObject.Document => FindDocument(),
            KnownDecompositionObject.Application => FindApplication(),
            KnownDecompositionObject.UiApplication => FindUiApplication(),
            KnownDecompositionObject.UiControlledApplication => FindUiControlledApplication(),
            KnownDecompositionObject.Database => FindDatabase(),
            KnownDecompositionObject.DependentElements => FindDependentElements(),
            KnownDecompositionObject.Selection => FindSelection(),
            KnownDecompositionObject.Face => FindFace(),
            KnownDecompositionObject.Edge => FindEdge(),
            KnownDecompositionObject.SubElement => FindSubElement(),
            KnownDecompositionObject.Point => FindPoint(),
            KnownDecompositionObject.LinkedElement => FindLinkedElement(),
            KnownDecompositionObject.ComponentManager => FindComponentManager(),
            KnownDecompositionObject.PerformanceAdviser => FindPerformanceAdviser(),
            KnownDecompositionObject.UpdaterRegistry => FindUpdaterRegistry(),
            KnownDecompositionObject.Services => FindServices(),
            KnownDecompositionObject.Schemas => FindSchemas(),
            _ => throw new ArgumentOutOfRangeException(nameof(decompositionObject), decompositionObject, null)
        };
    }

    private static IEnumerable FindView()
    {
        return new object?[] {Context.ActiveView};
    }

    private static IEnumerable FindDocument()
    {
        return new object?[] {Context.ActiveDocument};
    }

    private static IEnumerable FindApplication()
    {
        return new object?[] {Context.Application};
    }

    private static IEnumerable FindUiApplication()
    {
        return new object?[] {Context.UiApplication};
    }

    private static IEnumerable FindUiControlledApplication()
    {
        return new object[] {Context.UiControlledApplication};
    }

    private static IEnumerable FindEdge()
    {
        return FindObject(ObjectType.Edge);
    }

    private static IEnumerable FindFace()
    {
        return FindObject(ObjectType.Face);
    }

    private static IEnumerable FindSubElement()
    {
        return FindObject(ObjectType.Subelement);
    }

    private static IEnumerable FindPoint()
    {
        return FindObject(ObjectType.PointOnElement);
    }

    private static IEnumerable FindLinkedElement()
    {
        return FindObject(ObjectType.LinkedElement);
    }

    private static IEnumerable FindSelection()
    {
        var activeUiDocument = Context.ActiveUiDocument;
        if (activeUiDocument is null)
        {
            return Array.Empty<object>();
        }

        var selectedIds = activeUiDocument.Selection.GetElementIds();
        if (selectedIds.Count > 0)
        {
            return activeUiDocument.Document
                .GetElements(selectedIds)
                .WherePasses(new ElementIdSetFilter(selectedIds))
                .ToArray();
        }

        return activeUiDocument.Document
            .GetElements(activeUiDocument.ActiveView.Id)
            .ToArray();
    }

    private static IEnumerable FindDatabase()
    {
        var activeDocument = Context.ActiveDocument!;
        var elementTypes = activeDocument.GetElements().WhereElementIsElementType();
        var elementInstances = activeDocument.GetElements().WhereElementIsNotElementType();
        return elementTypes
            .UnionWith(elementInstances)
            .ToArray();
    }

    private static IEnumerable FindDependentElements()
    {
        var selectedIds = Context.ActiveUiDocument!.Selection.GetElementIds();
        if (selectedIds.Count == 0) return Array.Empty<object>();

        var elements = new List<ElementId>();
        var activeDocument = Context.ActiveDocument!;
        var selectedElements = activeDocument.GetElements(selectedIds).WhereElementIsNotElementType();

        foreach (var selectedElement in selectedElements)
        {
            var dependentElements = selectedElement.GetDependentElements(null);
            foreach (var dependentElement in dependentElements) elements.Add(dependentElement);
        }

        return activeDocument.GetElements()
            .WherePasses(new ElementIdSetFilter(elements))
            .ToArray();
    }

    private static IEnumerable FindComponentManager()
    {
        return new object?[] {typeof(ComponentManager)};
    }

    private static IEnumerable FindPerformanceAdviser()
    {
        return new object?[] {PerformanceAdviser.GetPerformanceAdviser()};
    }

    private static IEnumerable FindUpdaterRegistry()
    {
        return UpdaterRegistry.GetRegisteredUpdaterInfos();
    }

    private static IEnumerable FindSchemas()
    {
        return Schema.ListSchemas();
    }

    private static IEnumerable FindServices()
    {
        return ExternalServiceRegistry.GetServices();
    }

    private static IEnumerable FindObject(ObjectType objectType)
    {
        var activeUiDocument = Context.ActiveUiDocument;
        if (activeUiDocument is null)
        {
            return Array.Empty<object>();
        }

        var reference = activeUiDocument.Selection.PickObject(objectType);

        object element;
        switch (objectType)
        {
            case ObjectType.Edge:
            case ObjectType.Face:
                element = activeUiDocument.Document.GetElement(reference).GetGeometryObjectFromReference(reference);
                break;
            case ObjectType.Element:
            case ObjectType.Subelement:
                element = activeUiDocument.Document.GetElement(reference);
                break;
            case ObjectType.PointOnElement:
                element = reference.GlobalPoint;
                break;
            case ObjectType.LinkedElement:
                var revitLinkInstance = reference.ElementId.ToElement<RevitLinkInstance>(activeUiDocument.Document)!;
                element = revitLinkInstance.GetLinkDocument().GetElement(reference.LinkedElementId);
                break;
            case ObjectType.Nothing:
            default:
                throw new NotSupportedException();
        }

        return new[] {element};
    }
}