﻿using System.Windows;
using System.Windows.Controls;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using Color = System.Windows.Media.Color;

namespace RevitLookup2.Styles.ComponentStyles.ObjectsTree;

public sealed class TreeViewItemTemplateSelector : DataTemplateSelector
{
    /// <summary>
    ///     Tree view row style selector
    /// </summary>
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is null) return null;

        var presenter = (FrameworkElement) container;
        var decomposedObject = (ObservableDecomposedObject) item;
        var templateName = decomposedObject.RawValue switch
        {
            Color => "SummaryMediaColorItemTemplate",
            _ => "DefaultSummaryTreeItemTemplate"
        };

        return (DataTemplate) presenter.FindResource(templateName);
    }
}