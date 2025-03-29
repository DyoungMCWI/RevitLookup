// Copyright 2003-2024 by Autodesk, Inc.
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

using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using Autodesk.Revit.DB;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Configuration;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.UI.Framework.Extensions;
using RevitLookup.UI.Framework.Views.Visualization;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class CurveLoopDescriptor : Descriptor, IDescriptorResolver, IContextMenuConnector
{
    private readonly CurveLoop _curveLoop;

    public CurveLoopDescriptor(CurveLoop curveloop)
    {
        _curveLoop = curveloop;
        Name = $"{curveloop.GetExactLength().ToString(CultureInfo.InvariantCulture)} ft";
    }

    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(CurveLoop.IsOpen) => ResolveIsOpen,
            nameof(CurveLoop.GetPlane) => ResolveGetPlane,
            nameof(CurveLoop.NumberOfCurves) => ResolveNumberOfCurves,
            _ => null
        };

        IVariant ResolveNumberOfCurves()
        {
            var variants = Variants.Values<int>(1);

            variants.Add(_curveLoop.NumberOfCurves(), "number of curves in the curve loop");

            return variants.Consume();
        }

        IVariant ResolveIsOpen()
        {
            return Variants.Values<bool>(1)
                .Add(_curveLoop.IsOpen(), "whether the curve loop is open or closed")
                .Consume();
        }

        IVariant ResolveGetPlane()
        {
            return Variants.Values<Plane>(1)
                .Add(_curveLoop.GetPlane(), "Plane")
                .Consume();
        }
    }

    public void RegisterMenu(ContextMenu contextMenu, IServiceProvider serviceProvider)
    {
#if REVIT2023_OR_GREATER
        contextMenu.AddMenuItem("SelectMenuItem")
            .SetCommand(_curveLoop, SelectCurve)
            .SetShortcut(Key.F6);

        contextMenu.AddMenuItem("ShowMenuItem")
            .SetCommand(_curveLoop, ShowCurve)
            .SetShortcut(Key.F7);
#endif
        contextMenu.AddMenuItem("VisualizeMenuItem")
            .SetAvailability(_curveLoop.GetExactLength() > 1e-6)
            .SetCommand(_curveLoop, VisualizeCurve)
            .SetShortcut(Key.F8);

        async Task VisualizeCurve(CurveLoop curveloop)
        {
            if (Context.ActiveUiDocument is null) return;

            try
            {
                var dialog = serviceProvider.GetRequiredService<CurveLoopVisualizationDialog>();
                await dialog.ShowDialogAsync(curveloop);
            }
            catch (Exception exception)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<CurveLoopDescriptor>>();
                var notificationService = serviceProvider.GetRequiredService<INotificationService>();

                logger.LogError(exception, "Visualize curveloop error");
                notificationService.ShowError("Visualization error", exception);
            }
        }

#if REVIT2023_OR_GREATER
        void SelectCurve(CurveLoop curveloop)
        {
            if (Context.ActiveUiDocument is null) return;
            if (curveloop.Any(curve => curve.Reference is null)) return;

            foreach (var curve in curveloop)
            {
                RevitShell.ActionEventHandler.Raise(_ => Context.ActiveUiDocument.Selection.SetReferences([curve.Reference]));
            }
        }

        void ShowCurve(CurveLoop curveloop)
        {
            if (Context.ActiveUiDocument is null) return;
            if (curveloop.Any(curve => curve.Reference is null)) return;

            RevitShell.ActionEventHandler.Raise(application =>
            {
                var uiDocument = application.ActiveUIDocument;
                if (uiDocument is null) return;

                foreach (var curve in curveloop)
                {
                    var element = curve.Reference.ElementId.ToElement(uiDocument.Document);
                    if (element is not null) uiDocument.ShowElements(element);

                    uiDocument.Selection.SetReferences([curve.Reference]);
                }
            });
        }
#endif
    }
}