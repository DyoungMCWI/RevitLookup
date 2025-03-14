using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace RevitLookup.UI.Playground.Client.Controls;

[ContentProperty(nameof(ExampleContent))]
public sealed class ColorPageExample : UserControl
{
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(ColorPageExample), new PropertyMetadata(""));
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(ColorPageExample), new PropertyMetadata(""));
    public static readonly DependencyProperty ExampleContentProperty = DependencyProperty.Register(nameof(ExampleContent), typeof(UIElement), typeof(ColorPageExample), new PropertyMetadata(null));
        
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public UIElement ExampleContent
    {
        get => (UIElement)GetValue(ExampleContentProperty);
        set => SetValue(ExampleContentProperty, value);
    }

}