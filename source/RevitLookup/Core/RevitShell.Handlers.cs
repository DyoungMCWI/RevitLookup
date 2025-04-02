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
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Core;

public static partial class RevitShell
{
    private static ActionEventHandler? _actionEventHandler;
    private static AsyncEventHandler? _asyncEventHandler;
    private static AsyncEventHandler<ObservableDecomposedObject>? _asyncObjectHandler;
    private static AsyncEventHandler<List<ObservableDecomposedObject>>? _asyncObjectsHandler;
    private static AsyncEventHandler<List<ObservableDecomposedMember>>? _asyncMembersHandler;
    private static AsyncEventHandler<IEnumerable>? _asyncCollectionHandler;

    public static ActionEventHandler ActionEventHandler
    {
        get => _actionEventHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _actionEventHandler = value;
    }

    public static AsyncEventHandler AsyncEventHandler
    {
        get => _asyncEventHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncEventHandler = value;
    }

    public static AsyncEventHandler<ObservableDecomposedObject> AsyncObjectHandler
    {
        get => _asyncObjectHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncObjectHandler = value;
    }

    public static AsyncEventHandler<List<ObservableDecomposedObject>> AsyncObjectsHandler
    {
        get => _asyncObjectsHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncObjectsHandler = value;
    }

    public static AsyncEventHandler<List<ObservableDecomposedMember>> AsyncMembersHandler
    {
        get => _asyncMembersHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncMembersHandler = value;
    }

    public static AsyncEventHandler<IEnumerable> AsyncCollectionHandler
    {
        get => _asyncCollectionHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncCollectionHandler = value;
    }

    public static void RegisterHandlers()
    {
        ActionEventHandler = new ActionEventHandler();
        AsyncEventHandler = new AsyncEventHandler();
        AsyncObjectHandler = new AsyncEventHandler<ObservableDecomposedObject>();
        AsyncObjectsHandler = new AsyncEventHandler<List<ObservableDecomposedObject>>();
        AsyncMembersHandler = new AsyncEventHandler<List<ObservableDecomposedMember>>();
        AsyncCollectionHandler = new AsyncEventHandler<IEnumerable>();
    }
}