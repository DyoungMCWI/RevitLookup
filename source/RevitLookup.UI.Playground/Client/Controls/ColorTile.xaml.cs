using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;

namespace RevitLookup.UI.Playground.Client.Controls;

public sealed class ColorTile : UserControl
{
    public static readonly DependencyProperty TileRadiusProperty = DependencyProperty.Register(nameof(TileRadius), typeof(CornerRadius), typeof(ColorTile), new PropertyMetadata(new CornerRadius(0)));
    public static readonly DependencyProperty ColorNameProperty = DependencyProperty.Register(nameof(ColorName), typeof(string), typeof(ColorTile), new PropertyMetadata(""));
    public static readonly DependencyProperty ColorExplanationProperty = DependencyProperty.Register(nameof(ColorExplanation), typeof(string), typeof(ColorTile), new PropertyMetadata(""));
    public static readonly DependencyProperty ColorBrushNameProperty = DependencyProperty.Register(nameof(ColorBrushName), typeof(string), typeof(ColorTile), new PropertyMetadata(""));
    public static readonly DependencyProperty ColorValueProperty = DependencyProperty.Register(nameof(ColorValue), typeof(string), typeof(ColorTile), new PropertyMetadata(""));
    
    // Using a DependencyProperty as the backing store for ShowSeparator.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowSeparatorProperty = DependencyProperty.Register(nameof(ShowSeparator), typeof(bool), typeof(ColorTile), new PropertyMetadata(true));

    // Using a DependencyProperty as the backing store for ShowSeparator.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowWarningProperty = DependencyProperty.Register(nameof(ShowWarning), typeof(bool), typeof(ColorTile), new PropertyMetadata(false));
    
    static ColorTile()
    {
        CommandManager.RegisterClassCommandBinding(typeof(ColorTile), new CommandBinding(ApplicationCommands.Copy, OnCopyColorBrushClicked));
    }

    public CornerRadius TileRadius
    {
        get => (CornerRadius)GetValue(TileRadiusProperty);
        set => SetValue(TileRadiusProperty, value);
    }

    public string ColorName
    {
        get => (string)GetValue(ColorNameProperty);
        set => SetValue(ColorNameProperty, value);
    }

    public string ColorExplanation
    {
        get => (string)GetValue(ColorExplanationProperty);
        set => SetValue(ColorExplanationProperty, value);
    }

    public string ColorBrushName
    {
        get => (string)GetValue(ColorBrushNameProperty);
        set => SetValue(ColorBrushNameProperty, value);
    }

    public string ColorValue
    {
        get => (string)GetValue(ColorValueProperty);
        set => SetValue(ColorValueProperty, value);
    }

    public bool ShowSeparator
    {
        get => (bool)GetValue(ShowSeparatorProperty);
        set => SetValue(ShowSeparatorProperty, value);
    }

    public bool ShowWarning
    {
        get => (bool)GetValue(ShowWarningProperty);
        set => SetValue(ShowWarningProperty, value);
    }

    private static void OnCopyColorBrushClicked(object sender, RoutedEventArgs e)
    {
        if (sender is not ColorTile colorTile) return;
        if (string.IsNullOrEmpty(colorTile.ColorBrushName)) return;
        
        try
        {
            Clipboard.SetText(colorTile.ColorBrushName);
            var peer = UIElementAutomationPeer.CreatePeerForElement((ColorTile)e.OriginalSource);
            peer.RaiseNotificationEvent(
                AutomationNotificationKind.Other,
                AutomationNotificationProcessing.ImportantMostRecent,
                "Color Brush Name Copied",
                "ButtonClickedActivity"
            );

        }
        catch (Exception exception)
        {
            MessageBox.Show("Error copying to clipboard: " + exception.Message);
        }
    }
}