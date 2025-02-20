using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            foreach (var configuration in GlobBuildConfigurations())
            {
                AssemblyVersionsMap.TryGetValue(configuration, out var version);

                DotNetBuild(settings => settings
                    .SetConfiguration(configuration)
                    .SetVersion(version)
                    .SetProperty("IsRepackable", true)
                    .SetVerbosity(DotNetVerbosity.minimal));
            }
        });
}