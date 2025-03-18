using System.IO;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Options;
using RevitLookup.UI.Playground.Client.Extensions;

namespace RevitLookup.UI.Playground.Mockups.Config.Options;

public static class ResourcesOptions
{
    /// <summary>
    ///     Add add-in folders and file paths configuration
    /// </summary>
    public static void AddResourceLocationsOptions(this IServiceCollection services)
    {
        services.Configure<ResourceLocationsOptions>(options =>
        {
            options.ApplicationDataDirectory = Path
                .GetTempPath()
                .AppendPath("RevitLookup");

            options.LocalApplicationDataDirectory = Path
                .GetTempPath()
                .AppendPath("RevitLookup");

            //Local directories
            options.DownloadsFolder = options.LocalApplicationDataDirectory.AppendPath("DownloadCache");
            
            //Roaming directories
            options.SettingsDirectory = options.ApplicationDataDirectory.AppendPath("Settings");

            //Roaming files
            options.ApplicationSettingsPath = options.SettingsDirectory.AppendPath("Application.json");
            options.VisualizationSettingsPath = options.SettingsDirectory.AppendPath("Visualization.json");
        });
    }
}