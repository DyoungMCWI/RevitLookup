namespace RevitLookup.Abstractions.Models.AboutProgram;

/// <summary>
///     Represents information about open-source software.
/// </summary>
public sealed class OpenSourceSoftware
{
    public required string SoftwareName { get; set; }
    public required string SoftwareUri { get; set; }
    public required string LicenseName { get; set; }
    public required string LicenseUri { get; set; }
}