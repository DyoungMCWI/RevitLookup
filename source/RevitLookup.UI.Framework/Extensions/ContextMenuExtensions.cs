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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.UI.Framework.Utils;

namespace RevitLookup.UI.Framework.Extensions;

public static class ContextMenuExtensions
{
    public static void AddSeparator(this ContextMenu menu)
    {
        var separator = new Separator();
        menu.Items.Add(separator);
    }

    public static void AddLabel(this ContextMenu menu, string text)
    {
        var label = (MenuItem?) menu.Resources["Label"];
        if (label is null) throw new InvalidOperationException("Resource \"Label\" not found");

        label.Header = text;
        menu.Items.Add(label);
    }

    public static MenuItem AddMenuItem(this ContextMenu menu)
    {
        var item = new Wpf.Ui.Controls.MenuItem();
        menu.Items.Add(item);

        return item;
    }

    public static MenuItem AddMenuItem(this ContextMenu menu, string resource)
    {
        var item = (MenuItem?) menu.Resources[resource];
        if (item is null) throw new InvalidOperationException($"Resource \"{resource}\" not found");

        menu.Items.Add(item);

        return item;
    }

    public static MenuItem AddMenuItem(this MenuItem menu)
    {
        var item = new Wpf.Ui.Controls.MenuItem();
        menu.Items.Add(item);

        return item;
    }

    public static MenuItem AddMenuItem(this MenuItem menu, string resource)
    {
        var item = (MenuItem?) menu.FindLogicalParent<ContextMenu>()!.Resources[resource];
        if (item is null) throw new InvalidOperationException($"Resource \"{resource}\" not found");

        menu.Items.Add(item);

        return item;
    }

    public static MenuItem SetCommand(this MenuItem item, Action command)
    {
        item.Command = new RelayCommand(command);

        return item;
    }

    public static MenuItem SetCommand(this MenuItem item, ICommand command)
    {
        item.Command = command;

        return item;
    }

    public static MenuItem SetCommand(this MenuItem item, Func<Task> command)
    {
        item.Command = new AsyncRelayCommand(command);

        return item;
    }

    public static MenuItem SetCommand<T>(this MenuItem item, T parameter, Action<T> command)
    {
        item.CommandParameter = parameter;
        item.Command = new RelayCommand<T>(command!);

        return item;
    }

    public static MenuItem SetCommand<T>(this MenuItem item, T parameter, Func<T, Task> command)
    {
        item.CommandParameter = parameter;
        item.Command = new AsyncRelayCommand<T>(command!);

        return item;
    }

    public static MenuItem SetShortcut(this MenuItem item, ModifierKeys modifiers, Key key)
    {
        var inputGesture = new KeyGesture(key, modifiers);
        var menu = item.FindLogicalParent<ContextMenu>();
        if (menu is null) throw new InvalidOperationException("Unable to find context menu");

        menu.PlacementTarget.InputBindings.Add(new InputBinding(item.Command, inputGesture) {CommandParameter = item.CommandParameter});
        item.InputGestureText = inputGesture.GetDisplayStringForCulture(CultureInfo.InvariantCulture);

        return item;
    }

    public static MenuItem SetShortcut(this MenuItem item, Key key)
    {
        var inputGesture = new KeyGesture(key);
        var menu = item.FindLogicalParent<ContextMenu>();
        if (menu is null) throw new InvalidOperationException("Unable to find context menu");

        menu.PlacementTarget.InputBindings.Add(new InputBinding(item.Command, inputGesture) {CommandParameter = item.CommandParameter});
        item.InputGestureText = inputGesture.GetDisplayStringForCulture(CultureInfo.InvariantCulture);

        return item;
    }

    public static MenuItem SetHeader(this MenuItem item, string text)
    {
        item.Header = text;

        return item;
    }

    public static MenuItem SetChecked(this MenuItem item, bool state)
    {
        item.IsCheckable = true;
        item.IsChecked = state;

        return item;
    }

    public static MenuItem SetGestureText(this MenuItem item, Key key)
    {
        item.InputGestureText = new KeyGesture(key).GetDisplayStringForCulture(CultureInfo.InvariantCulture);

        return item;
    }

    public static MenuItem SetAvailability(this MenuItem item, bool condition)
    {
        item.SetCurrentValue(UIElement.IsEnabledProperty, condition);

        return item;
    }

    public static MenuItem SetStaysOpenOnClick(this MenuItem item, bool condition)
    {
        item.SetCurrentValue(MenuItem.StaysOpenOnClickProperty, condition);

        return item;
    }
}