using System.Diagnostics.CodeAnalysis;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Serilog.Events;

sealed partial class Build
{
    /// <summary>
    ///     Create the .msi installers.
    /// </summary>
    Target CreateInstaller => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            const string configuration = "Release";

            var target = Solution.Revit.RevitLookup;
            var installer = Solution.Automation.Installer;

            Log.Information("Project: {Name}", target.Name);

            var builderFile = Directory
                .EnumerateFiles(installer.Directory / "bin" / configuration, $"{installer.Name}.exe")
                .FirstOrDefault()
                .NotNull($"No installer builder was found for the project: {installer.Name}");

            var contentDirectories = Directory.GetDirectories(target.Directory / "bin", $"* {configuration} *", SearchOption.AllDirectories);
            Assert.NotEmpty(contentDirectories, "No content were found to create an installer");

            foreach (var contentDirectory in contentDirectories)
            {
                var process = ProcessTasks.StartProcess(builderFile, contentDirectory.DoubleQuoteIfNeeded(), logInvocation: false, logger: InstallerLogger);
                process.AssertZeroExitCode();
            }
        });

    /// <summary>
    ///     Logs the output of the installer process.
    /// </summary>
    [SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
    void InstallerLogger(OutputType outputType, string output)
    {
        if (outputType == OutputType.Err)
        {
            Log.Error(output);
            return;
        }

        var arguments = ArgumentsRegex.Matches(output);
        var logLevel = arguments.Count switch
        {
            0 => LogEventLevel.Debug,
            > 0 when output.Contains("error", StringComparison.OrdinalIgnoreCase) => LogEventLevel.Error,
            _ => LogEventLevel.Information
        };

        if (arguments.Count == 0)
        {
            Log.Write(logLevel, output);
            return;
        }

        var properties = arguments
            .Select(match => match.Value.Substring(1, match.Value.Length - 2))
            .Cast<object>()
            .ToArray();

        var messageTemplate = ArgumentsRegex.Replace(output, match => $"{{Property{match.Index}}}");
        Log.Write(logLevel, messageTemplate, properties);
    }
}