using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace RevitLookup.Config.Http;

public static class HttpClientConfiguration
{
    public static void AddHttpApiClients(this IServiceCollection services)
    {
        services.AddHttpClient("GitHubSource", client => client.BaseAddress = new Uri("https://api.github.com/repos/jeremytammik/RevitLookup/"));
        
        services.ConfigureHttpClientDefaults(builder =>
        {
            builder.RemoveAllLoggers();
        });
        
        services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
    }
}