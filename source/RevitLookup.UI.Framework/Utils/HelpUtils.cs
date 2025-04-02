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

using RevitLookup.Common.Tools;

namespace RevitLookup.UI.Framework.Utils;

public static class HelpUtils
{
    public static void ShowHelp(string query)
    {
        string uri;

        if (query.Contains(' '))
        {
            uri = $"https://duckduckgo.com/?q={query}";
        }
        else if (query.StartsWith("System"))
        {
            query = query.Replace('`', '-');
            uri = $"https://docs.microsoft.com/en-us/dotnet/api/{query}";
        }
        else
        {
            uri = $"https://duckduckgo.com/?q={query}";
        }

        ProcessTasks.StartShell(uri);
    }

    public static void ShowHelp(string query, string parameter)
    {
        if (query.StartsWith("System"))
        {
            ShowHelp($"{query}.{parameter}");
        }
        else
        {
            ShowHelp($"{query} {parameter}");
        }
    }
}