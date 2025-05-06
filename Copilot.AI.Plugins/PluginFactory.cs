using System.Text.Json;
using Copilot.AI.Plugins.Interfaces;
using Copilot.Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace Copilot.AI.Plugins;

public static class PluginFactory
{
    /// <summary>
    /// Creates a list of KernelPlugin objects from the given Copilot plugins.
    /// </summary>
    /// <param name="clientMessage">The client message.</param>
    /// <param name="dialogue">The dialogue object.</param>
    /// <param name="plugins">The list of plugins.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A list of KernelPlugin objects.</returns>
    public static IEnumerable<KernelPlugin> CreateFromCopilotPlugins(
        string clientMessage,
        Dialogue dialogue,
        List<Plugin> plugins,
        IServiceProvider serviceProvider)
    {
        var pluginTemplates = PluginTemplates.GetAllAsDictionary();

        var userPlugins = plugins.DistinctBy(x => x.Name);

        foreach (var plugin in userPlugins)
        {
            var generated = CreatePlugin(
                clientMessage: clientMessage,
                dialogue: dialogue,
                plugin: plugin,
                serviceProvider: serviceProvider,
                pluginTemplates: pluginTemplates
            );

            if (generated is null)
            {
                continue;
            }

            var kernelPlugin = KernelPluginFactory.CreateFromObject(
                pluginName: generated.Value.Name,
                target: generated.Value.Plugin,
                loggerFactory: serviceProvider.GetService<ILoggerFactory>()
            );

            yield return kernelPlugin;
        }
    }

    /// <summary>
    /// Creates a plugin object from the given parameters.
    /// </summary>
    /// <param name="clientMessage">The client message.</param>
    /// <param name="plugin">The plugin object.</param>
    /// <param name="dialogue">The dialogue object.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="pluginTemplates">The dictionary of plugin templates.</param>
    /// <returns>
    /// The name and ICopilotPlugin object if the creation is successful; otherwise, null.
    /// </returns>
    private static (string Name, ICopilotPlugin Plugin)? CreatePlugin(
        string clientMessage,
        Plugin plugin,
        Dialogue dialogue,
        IServiceProvider serviceProvider,
        Dictionary<string, PluginTemplate> pluginTemplates
    )
    {
        try
        {
            if (!pluginTemplates.TryGetValue(plugin.Name, out var pluginTemplate))
            {
                return null;
            }

            if (ActivatorUtilities.CreateInstance(serviceProvider, pluginTemplate.PluginType)
                is not ICopilotPlugin copilotPlugin)
            {
                return null;
            }

            var executionData = ParseExecutionData(plugin.ParameterData, pluginTemplate.ParameterDataType);
            executionData.ClientMessage = clientMessage;
            executionData.Dialogue = dialogue;

            copilotPlugin.SetParameters(executionData);

            return (pluginTemplate.Name, copilotPlugin);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Parses the execution data from the given parameter JSON string and returns an instance of ICopilotPlugin.ParameterBase.
    /// </summary>
    /// <param name="parameterJson">The parameter JSON string.</param>
    /// <param name="parameterType">The type of the parameter.</param>
    /// <returns>An instance of ICopilotPlugin.ParameterBase.</returns>
    private static ICopilotPlugin.ParameterBase ParseExecutionData(string parameterJson, Type parameterType)
    {
        return (JsonSerializer.Deserialize(parameterJson, parameterType)! as ICopilotPlugin.ParameterBase)!;
    }
}