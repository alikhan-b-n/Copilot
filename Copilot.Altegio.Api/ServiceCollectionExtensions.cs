using Copilot.Altegio.Api.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copilot.Altegio.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAltegioApi(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddTransient<IAltegioAPI, AltegioApi>();
        collection.AddTransient<IAltegioApiBuilder, AltegioApiBuilder>();
        
        return collection;
    }
}