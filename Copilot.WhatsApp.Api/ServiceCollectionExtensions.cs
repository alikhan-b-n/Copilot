using Copilot.WhatsApp.Api.Interfaces;
using Copilot.WhatsApp.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copilot.WhatsApp.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWhatsAppApi(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddScoped<IFlowSellApi, FlowSellApi>();

        return collection;
    }
}