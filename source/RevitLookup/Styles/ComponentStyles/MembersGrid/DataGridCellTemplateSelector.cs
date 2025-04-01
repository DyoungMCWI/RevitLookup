using System.Windows;
using System.Windows.Controls;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Styles.ComponentStyles.MembersGrid;

/// <summary>
///     Data grid cell template selector
/// </summary>
public sealed class DataGridCellTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is null) return null;

        var member = (ObservableDecomposedMember) item;
        var presenter = (FrameworkElement) container;
        var templateName = member.Value.RawValue switch
        {
            Color {IsValid: true} => "SummaryMediaColorCellTemplate",
            System.Windows.Media.Color => "SummaryMediaColorCellTemplate",
            _ => "DefaultSummaryCellTemplate"
        };

        return (DataTemplate) presenter.FindResource(templateName);
    }
}