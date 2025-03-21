// Copyright 2003-2024 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

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