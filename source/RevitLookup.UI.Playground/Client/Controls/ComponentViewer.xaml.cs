﻿using System.Windows;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace RevitLookup.UI.Playground.Client.Controls;

[PublicAPI]
public sealed partial class ComponentViewer
{
    private readonly IServiceProvider _serviceProvider;

    public ComponentViewer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    public bool? ShowComponent<T>() where T : UIElement
    {
        var page = _serviceProvider.GetRequiredService<T>();
        Viewer.Content = page;

        return ShowDialog();
    }
}