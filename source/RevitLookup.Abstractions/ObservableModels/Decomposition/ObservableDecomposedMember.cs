using CommunityToolkit.Mvvm.ComponentModel;
using LookupEngine.Abstractions.Enums;

namespace RevitLookup.Abstractions.ObservableModels.Decomposition;

/// <summary>
///     Represents the observable model for the LookupEngine decomposed member.
/// </summary>
public sealed class ObservableDecomposedMember : ObservableObject
{
    public required int Depth { get; set; }
    public required string Name { get; set; }
    public required string DeclaringTypeName { get; set; }
    public required string DeclaringTypeFullName { get; set; }
    public double ComputationTime { get; set; }
    public long AllocatedBytes { get; set; }
    public MemberAttributes MemberAttributes { get; set; }
    public required ObservableDecomposedValue Value { get; set; }
}