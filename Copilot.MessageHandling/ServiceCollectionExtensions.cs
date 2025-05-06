using Copilot.MessageHandling.Interfaces;
using Copilot.MessageHandling.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copilot.MessageHandling;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageHandlingServices(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection
            .AddScoped<IMessageSender, MessageSender>()
            .AddScoped<IMessageHandler, MessageHandler>()
            ;

        return collection;
    }
}