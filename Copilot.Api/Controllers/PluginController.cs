using Copilot.AI.Plugins.Plugins;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Copilot.Api.Controllers;

/// <summary>
/// Controller for managing plugins.
/// </summary>
[Controller]
public class PluginController(IPluginsManagementService pluginsManagementService) : ControllerBase
{
    /// <summary>
    /// Retrieves all plugins associated with a specified bot.
    /// </summary>
    /// <param name="botId">The identifier of the bot.</param>
    /// <returns>Returns a task that represents the asynchronous operation and contains a model representing all plugins.</returns>
    [HttpGet("api/chatbots/{botId:guid}/plugins")]
    public async Task<IActionResult> GetAll(Guid botId)
    {
        var response = await pluginsManagementService.GetAllPlugins(botId);
        
        return Ok(response);
    }

    /// <summary>
    /// Inserts or updates a salon plugin for a specified bot.
    /// </summary>
    /// <param name="botId">The identifier of the bot.</param>
    /// <param name="parameter">The parameters for the salon plugin.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    [HttpPut("api/chatbots/{botId:guid}/plugins/salon")]
    public async Task<IActionResult> UpsertSalonPlugin(Guid botId, [FromBody] SalonPluginParameter parameter)
    {
        await pluginsManagementService.UpsertSalonPlugin(botId, parameter);
        return Ok();
    }
    
    /// <summary>
    /// Inserts or updates a operator plugin for a specified bot.
    /// </summary>
    /// <param name="botId">The identifier of the bot.</param>
    /// <param name="parameter">The parameters for the salon plugin.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    [HttpPut("api/chatbots/{botId:guid}/plugins/operator")]
    public async Task<IActionResult> UpsertOperatorPlugin(Guid botId, [FromBody] OperatorPluginParameter parameter)
    {
        await pluginsManagementService.UpsertOperatorPlugin(botId, parameter);
        return Ok();
    }

    /// <summary>
    /// Deletes a plugin with the specified identifier.
    /// </summary>
    /// <param name="botId">The identifier of the bot.</param>
    /// <param name="id">The identifier of the plugin to delete.</param>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    [HttpDelete("api/chatbots/{botId:guid}/plugins/{id:guid}")]
    public async Task<IActionResult> Delete(Guid botId, Guid id)
    {
        await pluginsManagementService.DeletePlugin(id);
        return Ok();
    }
}
