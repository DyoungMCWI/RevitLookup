namespace RevitLookup.Abstractions.Models.UserInterface;

/// <summary>
///     Schema for the UI grouped cards. 
/// </summary>
public sealed class NavigationCardGroup
{
    /// <summary>
    ///     The group name.
    /// </summary>
    public required string GroupName { get; set; }

    /// <summary>
    ///     The list of navigation card items in the group.
    /// </summary>
    public required List<NavigationCardItem> Items { get; set; }
}