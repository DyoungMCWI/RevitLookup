﻿// Copyright 2003-2024 by Autodesk, Inc.
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

using System.Net.Http;
using System.Runtime;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RevitLookup.Abstractions.Options;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.States;
using RevitLookup.Abstractions.ViewModels.AboutProgram;
using RevitLookup.UI.Framework.Views.AboutProgram;

namespace RevitLookup.ViewModels.AboutProgram;

[UsedImplicitly]
public sealed partial class AboutViewModel : ObservableObject, IAboutViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISoftwareUpdateService _updateService;
    private readonly ILogger<AboutViewModel> _logger;

    [ObservableProperty] private SoftwareUpdateState _state = (SoftwareUpdateState) (-1);
    [ObservableProperty] private Version _currentVersion;
    [ObservableProperty] private string? _newVersion;
    [ObservableProperty] private string? _releaseNotesUrl;
    [ObservableProperty] private string? _latestCheckDate;
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private string _runtime;

    public AboutViewModel(
        IServiceProvider serviceProvider,
        ISoftwareUpdateService updateService,
        IOptions<AssemblyOptions> assemblyOptions,
        ILogger<AboutViewModel> logger)
    {
        _serviceProvider = serviceProvider;
        _updateService = updateService;
        _logger = logger;

        CurrentVersion = assemblyOptions.Value.Version;
        Runtime = new StringBuilder()
            .Append(assemblyOptions.Value.Framework)
            .Append(' ')
            .Append(Environment.Is64BitProcess ? "x64" : "x86")
            .Append(" (")
            .Append(GCSettings.IsServerGC ? "Server" : "Workstation")
            .Append(" GC)")
            .ToString();

        LatestCheckDate = _updateService.LatestCheckDate?.ToString("yyyy.MM.dd HH:mm:ss");
        UpdateSoftwareState();
    }

    [RelayCommand]
    private async Task CheckUpdatesAsync()
    {
        try
        {
            var result = await _updateService.CheckUpdatesAsync();

            if (!result)
            {
                State = SoftwareUpdateState.UpToDate;
                return;
            }

            UpdateSoftwareState();
        }
        catch (HttpRequestException exception)
        {
            State = SoftwareUpdateState.UpToDate;
            _logger.LogError(exception, "Checking updates fail");
        }
        catch (Exception exception)
        {
            State = SoftwareUpdateState.Error;
            ErrorMessage = "An unknown error occurred while checking for updates";
            _logger.LogError(exception, "Checking updates fail");
        }
        finally
        {
            LatestCheckDate = _updateService.LatestCheckDate?.ToString("yyyy.MM.dd HH:mm:ss");
        }
    }

    [RelayCommand]
    private async Task DownloadUpdateAsync()
    {
        try
        {
            await _updateService.DownloadUpdate();
            State = SoftwareUpdateState.ReadyToInstall;
        }
        catch (Exception exception)
        {
            State = SoftwareUpdateState.Error;
            ErrorMessage = "An error occurred while downloading the update";
            _logger.LogError(exception, "Downloading updates fail");
        }
    }

    [RelayCommand]
    private async Task ShowSoftwareDialogAsync()
    {
        var dialog = _serviceProvider.GetRequiredService<OpenSourceDialog>();
        await dialog.ShowAsync();
    }

    private void UpdateSoftwareState()
    {
        if (_updateService.LocalFilePath is not null)
        {
            State = SoftwareUpdateState.ReadyToInstall;
            return;
        }

        if (_updateService.NewVersion is null) return;

        NewVersion = _updateService.NewVersion;
        ReleaseNotesUrl = _updateService.ReleaseNotesUrl;
        State = SoftwareUpdateState.ReadyToDownload;
    }
}