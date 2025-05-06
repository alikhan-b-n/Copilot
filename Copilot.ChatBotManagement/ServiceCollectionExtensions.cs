using Copilot.ChatBotManagement.Implementations;
using Copilot.ChatBotManagement.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copilot.ChatBotManagement;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChatBotManagement(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddScoped<ICopilotChatBotService, CopilotChatBotService>();
        collection.AddScoped<IGptModelService,GptModelService>();
        collection.AddScoped<IFaqService, FaqService>();
        collection.AddScoped<IInstructionFileService, InstructionService>();
        collection.AddScoped<IDialogueService, DialogueService>();
        collection.AddScoped<IDialoguesService, DialoguesService>();
        collection.AddScoped<IPluginsManagementService, PluginsManagementService>();
        collection.AddScoped<IWhatsAppAccountService, WhatsAppAccountService>();
        collection.AddScoped<IAltegioCompanyService, AltegioCompanyService>();

        return collection;
    }
}