namespace RevitLookup.Abstractions.Models.EventArgs;

/// <summary>
///     Provides data for the global Application events.
/// </summary>
public sealed class EventInfoArgs
{
    /// <summary>
    ///     The event name.
    /// </summary>
    public required string EventName { get; set; }

    /// <summary>
    ///     The event arguments.
    /// </summary>
    public required object Arguments { get; set; }
}