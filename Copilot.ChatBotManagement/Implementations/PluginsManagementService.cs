using System.Text.Json;
using Calabonga.UnitOfWork;
using Copilot.AI.Plugins;
using Copilot.AI.Plugins.Plugins;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;

namespace Copilot.ChatBotManagement.Implementations;

public class PluginsManagementService(IUnitOfWork<ApplicationContext> unitOfWork) : IPluginsManagementService
{
    public async Task<AllPluginsModel> GetAllPlugins(Guid botId)
    {
        var result = new AllPluginsModel();

        var plugins = await unitOfWork
            .GetRepository<Plugin>()
            .GetAllAsync(predicate: x => x.CopilotChatBotId == botId);

        foreach (var plugin in plugins)
        {
            if (plugin.Name == PluginTemplates.SalonPlugin.Name)
            {
                result.SalonPluginId = plugin.Id;
                result.SalonPlugin = JsonSerializer.Deserialize<SalonPluginParameter>(plugin.ParameterData);
            }
            
            else if (plugin.Name == PluginTemplates.OperatorPlugin.Name)
            {
                result.OperatorPluginId = plugin.Id;
                result.OperatorPlugin = JsonSerializer.Deserialize<OperatorPluginParameter>(plugin.ParameterData);
            }
        }

        return result;
    }

    public async Task UpsertSalonPlugin(Guid botId, SalonPluginParameter salonPluginParameter)
    {
        // fetch the user plugin from DB
        var existingPlugin = await unitOfWork
            .GetRepository<Plugin>()
            .GetFirstOrDefaultAsync(predicate: x
                => x.CopilotChatBotId == botId
                   && x.Name == PluginTemplates.SalonPlugin.Name);

        if (existingPlugin != null)
        {
            // Update the existing plugin parameters
            existingPlugin.ParameterData = JsonSerializer.Serialize(salonPluginParameter);

            // update the plugin in DB
            unitOfWork.GetRepository<Plugin>().Update(existingPlugin);
        }
        else
        {
            // if the plugin does not exist then create a new one
            var newPlugin = new Plugin
            {
                CopilotChatBotId = botId,
                Name = PluginTemplates.SalonPlugin.Name,
                ParameterData = JsonSerializer.Serialize(salonPluginParameter)
            };

            // Add the new plugin to DB
            await unitOfWork.GetRepository<Plugin>().InsertAsync(newPlugin);
        }

        await unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpsertOperatorPlugin(Guid botId, OperatorPluginParameter operatorPluginParameter)
    {
        var existingPlugin = await unitOfWork.GetRepository<Plugin>()
            .GetFirstOrDefaultAsync(predicate: x
                => x.CopilotChatBotId == botId
                   && x.Name == PluginTemplates.OperatorPlugin.Name);
        
        if (existingPlugin != null)
        {
            // Update the existing plugin parameters
            existingPlugin.ParameterData = JsonSerializer.Serialize(operatorPluginParameter);

            // update the plugin in DB
            unitOfWork.GetRepository<Plugin>().Update(existingPlugin);
        }
        else
        {
            // if the plugin does not exist then create a new one
            var newPlugin = new Plugin
            {
                CopilotChatBotId = botId,
                Name = PluginTemplates.OperatorPlugin.Name,
                ParameterData = JsonSerializer.Serialize(operatorPluginParameter)
            };

            // Add the new plugin to DB
            await unitOfWork.GetRepository<Plugin>().InsertAsync(newPlugin);
        }

        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeletePlugin(Guid pluginId)
    {
        unitOfWork
            .GetRepository<Plugin>()
            .Delete(pluginId);

        await unitOfWork.SaveChangesAsync();
    }
}