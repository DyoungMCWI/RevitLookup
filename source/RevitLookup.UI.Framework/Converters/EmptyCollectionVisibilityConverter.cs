﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace RevitLookup.UI.Framework.Converters;

public sealed class EmptyCollectionVisibilityConverter : MarkupExtension, IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var count = (int) value!;
        return count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (Visibility) value! == Visibility.Visible;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}