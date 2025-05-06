using Copilot.Ai.Implementations;
using Copilot.Ai.Interfaces;
using Copilot.Ai.Models;
using Copilot.AI.Plugins;
using Copilot.Ai.Utils;
using Copilot.Ai.Utils.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copilot.Ai;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCopilotAi(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        // Variables will be helpful after KernelMemory Connection 
        var qdrantOptions = collection.ConfigureOptions<QdrantOptions>(configuration);
        var openAiModelOptions = collection.ConfigureOptions<OpenAiModelOptions>(configuration);
        
        collection
            .AddScoped<IAssistantRepository, AssistantRepository>()
            .AddScoped<IDialogueManager, DialogueManager>();

        collection
            .AddCopilotUtils(configuration)
            .AddCopilotPlugins(configuration);
        
        return collection;
    }
    
    private static T ConfigureOptions<T>(this IServiceCollection services, IConfiguration configuration)
        where T : class, new()
    {
        var options = new T();
        configuration
            .GetSection(typeof(T).Name)
            .Bind(options);

        services.Configure<T>(configuration.GetSection(typeof(T).Name));

        return options;
    }
}