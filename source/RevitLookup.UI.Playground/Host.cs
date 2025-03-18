using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.Abstractions.Services.Decomposition;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.UI.Framework.Services;
using RevitLookup.UI.Framework.Services.Presentation;
using RevitLookup.UI.Playground.Client.Services;
using RevitLookup.UI.Playground.Mockups.Config.Logging;
using RevitLookup.UI.Playground.Mockups.Config.Options;
using RevitLookup.UI.Playground.Mockups.Services.Appearance;
using RevitLookup.UI.Playground.Mockups.Services.Application;
using RevitLookup.UI.Playground.Mockups.Services.Decomposition;
using RevitLookup.UI.Playground.Mockups.Services.Settings;
using Wpf.Ui;
using Wpf.Ui.Abstractions;

namespace RevitLookup.UI.Playground;

/// <summary>
///     Provides a host for the application's services and manages their lifetimes.
/// </summary>
public static class Host
{
    private static readonly IServiceProvider ServiceProvider = RegisterServices();

    private static ServiceProvider RegisterServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(builder => builder.AddSerilogConfiguration());

        services.AddApplicationOptions();
        services.AddResourceLocationsOptions();
        services.AddSerializerOptions();

        //Frontend services
        services.AddScoped<INavigationViewPageProvider, DependencyInjectionNavigationViewPageProvider>();
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IContentDialogService, ContentDialogService>();
        services.AddScoped<ISnackbarService, SnackbarService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IWindowIntercomService, WindowIntercomService>();

        //MVVM services
        services.RegisterViews();
        services.RegisterViewModels();

        //Application Services
        services.AddSingleton<ISoftwareUpdateService, MockSoftwareUpdateService>();
        services.AddSingleton<ISettingsService, MockSettingsService>();
        services.AddSingleton<IThemeWatcherService, MockThemeWatcherService>();

        //Composer services
        services.AddScoped<IDecompositionService, MockDecompositionService>();
        services.AddScoped<IVisualDecompositionService, MockVisualDecompositionService>();
        services.AddScoped<IDecompositionSearchService, MockDecompositionSearchService>();
        services.AddTransient<IRevitLookupUiService, MockRevitLookupUiService>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    ///     Gets a service of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <returns>A service object of type T.</returns>
    public static T GetService<T>() where T : class
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    /// <summary>
    ///     Creates a window with the scope lifetime.
    /// </summary>
    /// <typeparam name="T">The type of window to get.</typeparam>
    /// <returns>A window of type T.</returns>
    public static T CreateScope<T>() where T : Window
    {
        var scopeFactory = ServiceProvider.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();

        var window = scope.ServiceProvider.GetRequiredService<T>();
        window.Closed += (_, _) => scope.Dispose();

        return window;
    }
}