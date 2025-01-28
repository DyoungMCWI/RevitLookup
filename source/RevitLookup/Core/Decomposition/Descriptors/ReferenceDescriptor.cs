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
using System.Windows.Controls;
using System.Windows.Input;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using RevitLookup.Abstractions.Configuration;
using RevitLookup.UI.Framework.Extensions;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class ReferenceDescriptor : Descriptor, IDescriptorResolver<Document>, IContextMenuConnector
{
    private readonly Reference _reference;

    public ReferenceDescriptor(Reference reference)
    {
        _reference = reference;
        Name = reference.ElementReferenceType.ToString();
    }

    public Func<Document, IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Reference.ConvertToStableRepresentation) => ResolveConvertToStableRepresentation,
            _ => null
        };

        IVariant ResolveConvertToStableRepresentation(Document context)
        {
            return Variants.Value(_reference.ConvertToStableRepresentation(context));
        }
    }

    public void RegisterMenu(ContextMenu contextMenu, IServiceProvider serviceProvider)
    {
#if REVIT2023_OR_GREATER
        contextMenu.AddMenuItem("SelectMenuItem")
            .SetCommand(_reference, SelectReference)
            .SetShortcut(Key.F6);

        void SelectReference(Reference reference)
        {
            if (Context.ActiveUiDocument is null) return;

            RevitShell.ActionEventHandler.Raise(_ => Context.ActiveUiDocument.Selection.SetReferences([reference]));
        }
#endif
    }
}