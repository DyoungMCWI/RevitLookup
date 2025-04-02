// Copyright (c) Lookup Foundation and Contributors
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// THIS PROGRAM IS PROVIDED "AS IS" AND WITH ALL FAULTS.
// NO IMPLIED WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE IS PROVIDED.
// THERE IS NO GUARANTEE THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RevitLookup.Abstractions.ObservableModels.Entries;

/// <summary>
///     Represents the observable model for the Revit INI entry.
/// </summary>
public sealed partial class ObservableIniEntry : ObservableValidator
{
    [ObservableProperty] [Required] [NotifyDataErrorInfo] private string _category = string.Empty;
    [ObservableProperty] [Required] [NotifyDataErrorInfo] private string _property = string.Empty;
    [ObservableProperty] private string _value = string.Empty;
    [ObservableProperty] private string? _defaultValue;
    [ObservableProperty] private bool _isActive;
    [ObservableProperty] private bool _isModified;

    public bool UserDefined { get; set; }

    public void Validate()
    {
        ValidateAllProperties();
    }

    partial void OnIsActiveChanged(bool value)
    {
        UserDefined = true;
    }

    partial void OnValueChanged(string value)
    {
        IsModified = DefaultValue is not null && value != DefaultValue;
    }

    partial void OnDefaultValueChanged(string? value)
    {
        IsModified = value != Value;
    }

    public ObservableIniEntry Clone()
    {
        return new ObservableIniEntry
        {
            Category = Category,
            Property = Property,
            Value = Value
        };
    }
}