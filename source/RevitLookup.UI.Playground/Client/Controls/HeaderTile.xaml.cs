using System.Diagnostics;
using System.Windows;

namespace RevitLookup.UI.Playground.Client.Controls;

public sealed partial class HeaderTile
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(HeaderTile), new PropertyMetadata(""));
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("ColorExplanation", typeof(string), typeof(HeaderTile), new PropertyMetadata(""));
    public static readonly DependencyProperty LinkProperty = DependencyProperty.Register(nameof(Link), typeof(string), typeof(HeaderTile), new PropertyMetadata(null));
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(HeaderTile), new PropertyMetadata(null));
        
    public HeaderTile()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
        
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string Link
    {
        get => (string)GetValue(LinkProperty);
        set => SetValue(LinkProperty, value);
    }

    public object Source
    {
        get => (object)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }


    private void OnTileClicked(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo(Link) { UseShellExecute = true });
    }
}