using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.AI.Plugins.Plugins;
using Copilot.AI.Plugins.Services.AltegioPlugin;
using Copilot.AI.Plugins.Services.AltegioPlugin.Components;
using Copilot.Ai.Utils;
using Copilot.Altegio.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copilot.AI.Plugins;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCopilotPlugins(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection
            .AddAltegioPlugins()
            .AddAltegioApi(configuration)
            .AddCopilotUtils(configuration)
            .AddScoped<SalonPlugin>()
            .AddScoped<OperatorPlugin>();

        return collection;
    }

    private static IServiceCollection AddAltegioPlugins(this IServiceCollection services)
    {
        // Add Components
        services
            .AddTransient<Dictionary<string, IAltegioComponent>>(provider => new Dictionary<string, IAltegioComponent>
            {
                { nameof(GetDateByQueryComponent), provider.GetRequiredService<GetDateByQueryComponent>() },
                { nameof(GetIdsByQueryComponent), provider.GetRequiredService<GetIdsByQueryComponent>() },
                { nameof(GetSlotsComponent), provider.GetRequiredService<GetSlotsComponent>() },
                { nameof(TryMakeAppointmentComponent), provider.GetRequiredService<TryMakeAppointmentComponent>() },
                { nameof(GetAllMasersComponent), provider.GetRequiredService<GetAllMasersComponent>() },
                { nameof(TransferAppointmentComponent), provider.GetRequiredService<TransferAppointmentComponent>() },
                { nameof(BookRecordComponent), provider.GetRequiredService<BookRecordComponent>() },
                { nameof(CancelAppointmentComponent), provider.GetRequiredService<CancelAppointmentComponent>() },
                { nameof(GetAllCategoriesComponent), provider.GetRequiredService<GetAllCategoriesComponent>() },
                { nameof(GetAllServicesComponent), provider.GetRequiredService<GetAllServicesComponent>() },
                { nameof(GetAppointmentsComponent), provider.GetRequiredService<GetAppointmentsComponent>() },
            })
            .AddTransient<GetAllMasersComponent>()
            .AddTransient<GetDateByQueryComponent>()
            .AddTransient<GetIdsByQueryComponent>()
            .AddTransient<GetSlotsComponent>()
            .AddTransient<TryMakeAppointmentComponent>()
            .AddTransient<TransferAppointmentComponent>()
            .AddTransient<BookRecordComponent>()
            .AddTransient<CancelAppointmentComponent>()
            .AddTransient<GetAllCategoriesComponent>()
            .AddTransient<GetAllServicesComponent>()
            .AddTransient<GetAppointmentsComponent>()
            ;

        services.AddDistributedMemoryCache();

        return services
            .AddTransient<SalonPlugin>()
            .AddScoped<IAltegioPluginService, AltegioPluginService>();
    }
}