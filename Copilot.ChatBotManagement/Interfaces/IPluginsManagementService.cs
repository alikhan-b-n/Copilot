using Copilot.AI.Plugins.Plugins;
using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;

namespace Copilot.ChatBotManagement.Interfaces;

/// <summary>
/// Interface for managing plugins.
/// </summary>
public interface IPluginsManagementService
{
    /// <summary>
    /// Retrieves all plugins associated with a specified bot.
    /// </summary>
    /// <param name="botId">The identifier of the bot.</param>
    /// <returns>Returns a task that represents the asynchronous operation and contains a model representing all plugins.</returns>
    public Task<AllPluginsModel> GetAllPlugins(Guid botId);

    /// <summary>
    /// Upserts a salon plugin for a specified bot.
    /// </summary>
    /// <param name="botId">The identifier of the bot.</param>
    /// <param name="salonPluginParameter">The parameters for the salon plugin.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public Task UpsertSalonPlugin(Guid botId, SalonPluginParameter salonPluginParameter);

    /// <summary>
    /// Upserts a operator plugin for a specified.
    /// </summary>
    /// <param name="botId">The identifier of the bot.</param>
    /// <param name="operatorPluginParameter">The parameters for the operator plugin.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public Task UpsertOperatorPlugin(Guid botId, OperatorPluginParameter operatorPluginParameter);

    /// <summary>
    /// Deletes a plugin with the specified identifier.
    /// </summary>
    /// <param name="pluginId">The identifier of the plugin to delete.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public Task DeletePlugin(Guid pluginId);
}
