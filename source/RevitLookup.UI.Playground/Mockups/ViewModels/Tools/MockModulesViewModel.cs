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


using System.Runtime.Loader;
using CommunityToolkit.Mvvm.ComponentModel;
using RevitLookup.Abstractions.Models.Tools;
using RevitLookup.Abstractions.ViewModels.Tools;
#if NETFRAMEWORK
using RevitLookup.UI.Framework.Extensions;
#endif

namespace RevitLookup.UI.Playground.Mockups.ViewModels.Tools;

public sealed partial class MockModulesViewModel : ObservableObject, IModulesViewModel
{
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private List<ModuleInfo> _modules = [];
    [ObservableProperty] private List<ModuleInfo> _filteredModules = [];

    public MockModulesViewModel()
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
                // ReSharper disable once LoopCanBeConvertedToQuery
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