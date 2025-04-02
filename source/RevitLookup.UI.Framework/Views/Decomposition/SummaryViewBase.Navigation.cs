﻿// Copyright (c) Lookup Foundation and Contributors
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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LookupEngine.Abstractions.Configuration;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.UI.Framework.Utils;

namespace RevitLookup.UI.Framework.Views.Decomposition;

public partial class SummaryViewBase
{
    /// <summary>
    ///     Handle tree view select event
    /// </summary>
    /// <remarks>
    ///     Collect data for selected item
    /// </remarks>
    private void OnTreeItemSelected(object sender, RoutedPropertyChangedEventArgs<object> args)
    {
        switch (args.NewValue)
        {
            case ObservableDecomposedObject decomposedObject:
                ViewModel.SelectedDecomposedObject = decomposedObject;
                break;
            case ObservableDecomposedObjectsGroup:
            case null:
                ViewModel.SelectedDecomposedObject = null;
                break;
            default:
                return;
        }
    }

    /// <summary>
    ///     Handle tree view click event
    /// </summary>
    /// <remarks>
    ///     Navigate on Ctrl pressed
    /// </remarks>
    private void OnTreeItemClicked(object sender, RoutedEventArgs args)
    {
        if ((Keyboard.Modifiers & ModifierKeys.Control) == 0) return;
        args.Handled = true;

        var element = (FrameworkElement) args.OriginalSource;
        switch (element.DataContext)
        {
            case ObservableDecomposedObject item:
                ViewModel.Navigate(item);
                break;
            case ObservableDecomposedObjectsGroup group:
                ViewModel.Navigate(group.GroupItems.ToList());
                break;
        }
    }

    /// <summary>
    ///     Handle data grid click event
    /// </summary>
    /// <remarks>
    ///     Navigate on row clicked
    /// </remarks>
    private void OnGridRowClicked(object sender, RoutedEventArgs args)
    {
        var row = (DataGridRow) sender;
        if (row.DataContext is not ObservableDecomposedMember context) return;

        if ((Keyboard.Modifiers & ModifierKeys.Control) == 0)
        {
            if (context.Value.Descriptor is not IDescriptorCollector) return;
            if (context.Value.Descriptor is IDescriptorEnumerator {IsEmpty: true}) return;
        }

        ViewModel.Navigate(context.Value);
    }

    /// <summary>
    ///     Handle cursor interaction
    /// </summary>
    private static void OnPresenterCursorInteracted(object sender, MouseEventArgs args)
    {
        var presenter = (FrameworkElement) sender;
        if ((Keyboard.Modifiers & ModifierKeys.Control) == 0)
        {
            presenter.Cursor = null;
            return;
        }

        FrameworkElement? item = sender switch
        {
            DataGrid => ((DependencyObject) args.OriginalSource).FindVisualParent<DataGridRow>(),
            TreeView => ((DependencyObject) args.OriginalSource).FindVisualParent<TreeViewItem>(),
            _ => throw new NotSupportedException()
        };

        if (item is null)
        {
            presenter.Cursor = null;
            return;
        }

        presenter.Cursor = Cursors.Hand;
        presenter.PreviewKeyUp += OnPresenterCursorRestored;
    }

    /// <summary>
    ///     Restore cursor
    /// </summary>
    private static void OnPresenterCursorRestored(object sender, KeyEventArgs e)
    {
        var presenter = (FrameworkElement) sender;
        presenter.PreviewKeyUp -= OnPresenterCursorRestored;
        presenter.Cursor = null;
    }
}