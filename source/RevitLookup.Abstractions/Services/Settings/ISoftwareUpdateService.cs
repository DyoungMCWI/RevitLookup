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

namespace RevitLookup.Abstractions.Services.Settings;

/// <summary>
///     Software update provider.
/// </summary>
public interface ISoftwareUpdateService
{
    /// <summary>
    ///     A new available version to download.
    /// </summary>
    public string? NewVersion { get; }
    
    /// <summary>
    ///     The URL to the release notes of the new version.
    /// </summary>
    public string? ReleaseNotesUrl { get; }
    
    /// <summary>
    ///     The local file path to the downloaded update.
    /// </summary>
    public string? LocalFilePath { get; }
    
    /// <summary>
    ///     The date of the latest check for updates.
    /// </summary>
    public DateTime? LatestCheckDate { get; }

    /// <summary>
    ///     Check for updates on the server.
    /// </summary>
    Task<bool> CheckUpdatesAsync();
    
    /// <summary>
    ///     Download the update from the server.
    /// </summary>
    Task DownloadUpdate();
}