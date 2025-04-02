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
using System.Windows.Controls;
using System.Windows.Input;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Configuration;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.ViewModels.Decomposition;
using RevitLookup.UI.Framework.Extensions;
using RevitLookup.UI.Framework.Views.EditDialogs;
using Wpf.Ui.Controls;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class ParameterDescriptor : Descriptor, IDescriptorResolver, IDescriptorExtension, IContextMenuConnector
{
    private readonly Parameter _parameter;

    public ParameterDescriptor(Parameter parameter)
    {
        _parameter = parameter;
        Name = parameter.Definition.Name;
    }

    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Parameter.ClearValue) => Variants.Disabled,
            _ => null
        };
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
        if (_parameter.StorageType == StorageType.Integer)
        {
            manager.Register(nameof(ParameterExtensions.AsBool), () => Variants.Value(_parameter.AsBool()));
            manager.Register(nameof(ParameterExtensions.AsColor), () => Variants.Value(_parameter.AsColor()));
        }

        if (_parameter.Element.Document.IsFamilyDocument)
        {
            manager.Register(nameof(FamilyManager.GetAssociatedFamilyParameter), RegisterGetAssociatedFamilyParameter);
        }

        return;

        IVariant RegisterGetAssociatedFamilyParameter()
        {
            return Variants.Value(_parameter.Element.Document.FamilyManager.GetAssociatedFamilyParameter(_parameter));
        }
    }

    public void RegisterMenu(ContextMenu contextMenu, IServiceProvider serviceProvider)
    {
        contextMenu.AddMenuItem("EditMenuItem")
            .SetHeader("Edit value")
            .SetAvailability(!_parameter.IsReadOnly && _parameter.StorageType != StorageType.None)
            .SetCommand(_parameter, EditParameter)
            .SetShortcut(Key.F2);

        return;

        async Task EditParameter(Parameter parameter)
        {
            try
            {
                var dialog = serviceProvider.GetRequiredService<EditValueDialog>();
                var result = await dialog.ShowAsync(parameter.Definition.Name, RevitShell.GetParameterValue(parameter), "Update the parameter");
                if (result == ContentDialogResult.Primary)
                {
                    var parameterValue = dialog.Value; // Share between threads
                    await RevitShell.AsyncEventHandler.RaiseAsync(_ => RevitShell.UpdateParameterValue(parameter, parameterValue));

                    var decompositionViewModel = serviceProvider.GetRequiredService<IDecompositionSummaryViewModel>();
                    await decompositionViewModel.RefreshMembersAsync();
                }
            }
            catch (Exception exception)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ParameterDescriptor>>();
                var notificationService = serviceProvider.GetRequiredService<INotificationService>();

                logger.LogError(exception, "Update value error");
                notificationService.ShowError("Updating parameter value error", exception);
            }
        }
    }
}