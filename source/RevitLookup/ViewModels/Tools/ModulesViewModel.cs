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


using System.Diagnostics.CodeAnalysis;
using RevitLookup.Abstractions.Models.Tools;
using RevitLookup.Abstractions.ViewModels.Tools;
#if NETCOREAPP
using System.Runtime.Loader;
#endif

namespace RevitLookup.ViewModels.Tools;

[UsedImplicitly]
public sealed partial class ModulesViewModel : ObservableObject, IModulesViewModel
{
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private List<ModuleInfo> _modules = [];
    [ObservableProperty] private List<ModuleInfo> _filteredModules = [];

    public ModulesViewModel()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Modules = new List<ModuleInfo>(assemblies.Length);

        for (var i = 0; i < assemblies.Length; i++)
        {
            var assembly = assemblies[i];
            var assemblyName = assembly.GetName();
            var module = new ModuleInfo
            {
                Name = assemblyName.Name ?? string.Empty,
                Path = assembly.IsDynamic ? string.Empty : assembly.Location,
                Order = i + 1,
                Version = assemblyName.Version is null ? string.Empty : assemblyName.Version.ToString(),
#if NETCOREAPP
                Container = AssemblyLoadContext.GetLoadContext(assembly)?.Name ?? string.Empty
#else
                Container = AppDomain.CurrentDomain.FriendlyName
#endif
            };

            Modules.Add(module);
        }
    }

    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
    async partial void OnSearchTextChanged(string value)
    {
        try
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredModules = Modules;
                return;
            }

            FilteredModules = await Task.Run(() =>
            {
                var formattedText = value.Trim();
                var searchResults = new List<ModuleInfo>();
                foreach (var module in Modules)
                {
                    if (module.Name.Contains(formattedText, StringComparison.OrdinalIgnoreCase) ||
                        module.Path.Contains(formattedText, StringComparison.OrdinalIgnoreCase) ||
                        module.Version.Contains(formattedText, StringComparison.OrdinalIgnoreCase))
                    {
                        searchResults.Add(module);
                    }
                }

                return searchResults;
            });
        }
        catch
        {
            // ignored
        }
    }

    partial void OnModulesChanged(List<ModuleInfo> value)
    {
        FilteredModules = value;
    }
}