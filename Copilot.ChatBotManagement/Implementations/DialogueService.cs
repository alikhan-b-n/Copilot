using Calabonga.UnitOfWork;
using Copilot.Ai.Interfaces;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Copilot.ChatBotManagement.Implementations;

public class DialogueService(
    IUnitOfWork<ApplicationContext> unitOfWork,
    IAssistantRepository assistantRepository,
    IDialogueManager dialogueManager) : IDialogueService
{
    public async Task<string> ProcessMessage(NewMessageDialogueModelRequest request)
    {
        var chatBot = await GetChatBotEntity(request.ChatBotId);
        var dialogue = await GetDialogueOrDefault(request.ExternalIdentifier, chatBot, request.PhoneNumber)
                       ?? await CreateNewDialogue(request.ExternalIdentifier, chatBot, request.PhoneNumber);

        if (!dialogue.IsActive)
        {
            return string.Empty;
        }
        
        var response = await GiveAnswer(dialogue, request.MessageContent);

        return response;
    }

    private async Task<Dialogue?> GetDialogueOrDefault(string externalIdentifier, CopilotChatBot chatBot,
        string? phoneNumber)
    {
        var dialogue = await unitOfWork
            .GetRepository<Dialogue>()
            .GetAll(disableTracking: false)
            .Where(x => x.ExternalIdentifier == externalIdentifier)
            .Where(x => x.CopilotChatBotId == chatBot.Id)
            .FirstOrDefaultAsync();

        if (dialogue == default)
        {
            return dialogue;
        }

        if (string.IsNullOrEmpty(dialogue.ThreadId))
        {
            var thread = await assistantRepository.CreateThreadAsync(chatBot.AssistantId);
            dialogue.ThreadId = thread.Id;

            unitOfWork.GetRepository<Dialogue>().Update(dialogue);
            await unitOfWork.SaveChangesAsync();
        }

        if (!string.IsNullOrEmpty(phoneNumber) && string.IsNullOrEmpty(dialogue.PhoneNumber))
        {
            dialogue.PhoneNumber = phoneNumber;
            unitOfWork.GetRepository<Dialogue>().Update(dialogue);
            await unitOfWork.SaveChangesAsync();
        }

        dialogue.CopilotChatBot = chatBot;

        return dialogue;
    }

    private async Task<Dialogue> CreateNewDialogue(string externalIdentifier, CopilotChatBot chatBot,
        string? phoneNumber)
    {
        if (string.IsNullOrEmpty(chatBot.AssistantId))
        {
            var assistant = await assistantRepository.CreateAsync(
                gptModel: chatBot.GptModel.Name
            );

            chatBot.AssistantId = assistant.Id;
        }

        var thread = await assistantRepository.CreateThreadAsync(chatBot.AssistantId);

        var newDialogue = new Dialogue
        {
            CopilotChatBot = chatBot,
            CopilotChatBotId = chatBot.Id,
            ExternalIdentifier = externalIdentifier,
            ThreadId = thread.Id,
            PhoneNumber = phoneNumber ?? string.Empty
        };

        await unitOfWork.GetRepository<Dialogue>().InsertAsync(newDialogue);
        await unitOfWork.SaveChangesAsync();

        return newDialogue;
    }

    private async Task<CopilotChatBot> GetChatBotEntity(Guid chatBotId)
    {
        return await unitOfWork
            .GetRepository<CopilotChatBot>()
            .GetAll(disableTracking: false)
            .Where(x => x.Id == chatBotId)
            .Include(x => x.GptModel)
            .FirstOrDefaultAsync() ?? throw new ArgumentException("Could not find chatbot", nameof(chatBotId));
    }

    private async Task<string> GiveAnswer(Dialogue dialogue, string messageContent)
    {
        var plugins = await unitOfWork
            .GetRepository<Plugin>()
            .GetAll(predicate: x => x.CopilotChatBotId == dialogue.CopilotChatBotId)
            .ToListAsync();

        var answer = await dialogueManager.GiveAnswer(
            new DialogueManagerParameter(
                IncomingMessage: messageContent,
                Dialogue: dialogue,
                Plugins: plugins
            )
        );

        return answer.Content;
    }
}