using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Options;

namespace RevitLookup.Config.Options;

public static class ResourcesOptions
{
    /// <summary>
    ///     Add add-in folders and file paths configuration
    /// </summary>
    public static void AddResourceLocationsOptions(this IServiceCollection services)
    {
        services.Configure<ResourceLocationsOptions>(options =>
        {
            options.ApplicationDataDirectory = Environment
                .GetFolderPath(Environment.SpecialFolder.ApplicationData)
                .AppendPath("RevitLookup")
                .AppendPath(Context.Application.VersionNumber);

            options.LocalApplicationDataDirectory = Environment
                .GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                .AppendPath("RevitLookup")
                .AppendPath(Context.Application.VersionNumber);

            //Local directories
            options.DownloadsFolder = options.LocalApplicationDataDirectory.AppendPath("DownloadCache");
            
            //Roaming directories
            options.SettingsDirectory = options.ApplicationDataDirectory.AppendPath("Settings");

            //Roaming files
            options.ApplicationSettingsPath = options.SettingsDirectory.AppendPath("Application.json");
            options.DecompositionSettingsPath = options.SettingsDirectory.AppendPath("LookupEngine.json");
            options.VisualizationSettingsPath = options.SettingsDirectory.AppendPath("Visualization.json");
        });
    }
}