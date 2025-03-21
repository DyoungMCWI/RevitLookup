using System.Windows.Input;
using Wpf.Ui.Controls;

namespace RevitLookup.Abstractions.Models.UserInterface;

/// <summary>
///     Schema for the UI card item.
/// </summary>
public sealed class NavigationCardItem
{
    /// <summary>
    ///     The card title.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    ///     The card description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     The card icon.
    /// </summary>
    public required SymbolRegular Icon { get; set; }

    /// <summary>
    ///     The command to execute when the card is clicked.
    /// </summary>
    public required ICommand Command { get; set; }

    /// <summary>
    ///     The parameter to pass to the command.
    /// </summary>
    public object? CommandParameter { get; set; }
}