using Copilot.Ai.Utils.Interfaces;
using Copilot.Ai.Utils.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copilot.Ai.Utils;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCopilotUtils(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddScoped<IGptTextEmitter, GptTextEmitter>();
        collection.AddScoped<IKernelFactory, KernelFactory>();
        
        return collection;
    }
}