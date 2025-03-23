using Nuke.Common.Tools.AzureSignTool;

sealed partial class Build
{
    /// <summary>
    ///     Sign the compiled assemblies.
    /// </summary>
    Target SignAssembly => _ => _
        .DependsOn(Compile)
        .Before(CreateInstaller)
        .Requires(() => AzureVaultUri)
        .Requires(() => AzureVaultTenantId)
        .Requires(() => AzureVaultClientId)
        .Requires(() => AzureVaultClientSecret)
        .Requires(() => AzureVaultCertificateName)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            const string configuration = "Release";
            var signTarget = Solution.Revit.RevitLookup;

            var targetDirectories = signTarget.Directory.GetDirectories($"* {configuration} *", 4);
            var targetFiles = targetDirectories
                .SelectMany(directory => directory.GetFiles(depth: 2))
                .ToArray();

            Assert.NotEmpty(targetFiles, "No files were found to sign");

            Sign(targetFiles);
        });

    /// <summary>
    ///    Sign installers.
    /// </summary>
    Target SignInstaller => _ => _
        .DependsOn(CreateInstaller)
        .Requires(() => AzureVaultUri)
        .Requires(() => AzureVaultTenantId)
        .Requires(() => AzureVaultClientId)
        .Requires(() => AzureVaultClientSecret)
        .Requires(() => AzureVaultCertificateName)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            var targetFiles = ArtifactsDirectory
                .GetFiles()
                .ToArray();

            Assert.NotEmpty(targetFiles, "No installers were found to sign");

            Sign(targetFiles);
        });

    void Sign(AbsolutePath[] files)
    {
        var validFiles = files
            .Where(file => DateTime.UtcNow - File.GetLastWriteTimeUtc(file) < TimeSpan.FromHours(1))
            .Where(file => file.Extension is ".dll" or ".msi")
            .Select(path => path.ToString())
            .ToArray();

        Log.Information("Signing {Count} files", validFiles.Length);

        var inputFile = Path.GetTempFileName();
        File.WriteAllLines(inputFile, validFiles);

        AzureSignToolTasks.AzureSignTool(_ => _
            .SetKeyVaultUrl(AzureVaultUri)
            .SetKeyVaultTenantId(AzureVaultTenantId)
            .SetKeyVaultClientId(AzureVaultClientId)
            .SetKeyVaultClientSecret(AzureVaultClientSecret)
            .SetKeyVaultCertificateName(AzureVaultCertificateName)
            .SetTimestampRfc3161Url("http://timestamp.digicert.com")
            .SetSkipSigned(true)
            .SetContinueOnError(true)
            .SetInputFileList(inputFile));
    }
}