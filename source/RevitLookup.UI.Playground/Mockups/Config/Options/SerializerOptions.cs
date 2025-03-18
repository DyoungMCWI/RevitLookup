using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace RevitLookup.UI.Playground.Mockups.Config.Options;

public static class SerializerOptions
{
    /// <summary>
    ///    Add global JsonSerialization configuration/>
    /// </summary>
    public static void AddSerializerOptions(this IServiceCollection services)
    {
        services.Configure<JsonSerializerOptions>(options =>
        {
#if DEBUG
            options.WriteIndented = true;
#endif
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.Converters.Add(new JsonStringEnumConverter());
        });
    }
}