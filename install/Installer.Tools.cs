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

using System.Diagnostics;
using System.Runtime.InteropServices;
using WixSharp.CommonTasks;

namespace Installer;

/// <summary>
///     Installer versions metadata.
/// </summary>
public sealed class Versions
{
    public required Version InstallerVersion { get; init; }
    public required Version AssemblyVersion { get; init; }
    public int RevitVersion { get; init; }
}

public static class Tools
{
    /// <summary>
    ///     Compute installer versions based on the RevitLookup.dll file.
    /// </summary>
    public static Versions ComputeVersions(string[] args)
    {
        foreach (var directory in args)
        {
            var assemblies = Directory.GetFiles(directory, "RevitLookup.dll", SearchOption.AllDirectories);
            if (assemblies.Length == 0) continue;

            var projectAssembly = FileVersionInfo.GetVersionInfo(assemblies[0]);
            var version = new Version(projectAssembly.FileVersion).ClearRevision();
            return new Versions
            {
                AssemblyVersion = version,
                RevitVersion = version.Major,
                InstallerVersion = version.Major > 255 ? new Version(version.Major % 100, version.Minor, version.Build) : version
            };
        }

        throw new Exception("RevitLookup.dll file could not be found");
    }
}