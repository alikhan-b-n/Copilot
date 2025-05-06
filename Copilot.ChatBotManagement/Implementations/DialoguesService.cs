using Calabonga.UnitOfWork;
using Copilot.Ai.Interfaces;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Responses;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Copilot.ChatBotManagement.Implementations;

public class DialoguesService(
    IUnitOfWork<ApplicationContext> unitOfWork,
    IAssistantRepository assistantRepository)
    : IDialoguesService
{
    /// <summary>
    /// Retrieves all dialogues.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="botId">The bot ID.</param>
    /// <param name="showTest">Flag indicating whether to include test dialogues.</param>
    /// <returns>A list of dialogue responses.</returns>
    public async Task<List<DialogueResponse>> GetAll(Guid userId, Guid botId, bool showTest = false)
    {
        var result = await unitOfWork
            .GetRepository<Dialogue>()
            .GetAll(
                predicate: x =>
                    x.CopilotChatBotId == botId
                    && x.CopilotChatBot.UserId == userId
                    && (!showTest || x.ExternalIdentifier.StartsWith("test_"))!,
                include: x => x.Include(d => d.CopilotChatBot)
            )
            .Where(x => showTest || !x.ExternalIdentifier.StartsWith("test_"))
            .OrderByDescending(x => x.CreationDate)
            .Select(x => new DialogueResponse
            {
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                IsActive = x.IsActive,
                CreatedAt = x.CreationDate
            })
            .ToListAsync();

        return result;
    }

    /// <summary>
    /// Changes the status of a dialogue to active or inactive.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="botId">The bot ID.</param>
    /// <param name="dialogueId">The dialogue ID.</param>
    /// <returns></returns>
    public async Task ChangeStatus(Guid userId, Guid botId, Guid dialogueId)
    {
        var dialogue = await unitOfWork
            .GetRepository<Dialogue>()
            .GetFirstOrDefaultAsync(
                disableTracking: false,
                predicate: x =>
                    x.CopilotChatBotId == botId
                    && x.CopilotChatBot.UserId == userId
                    && x.Id == dialogueId,
                include: x => x.Include(d => d.CopilotChatBot)
            );

        if (dialogue is null)
        {
            return;
        }

        dialogue.IsActive = !dialogue.IsActive;
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a dialogue.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="botId">The bot ID.</param>
    /// <param name="dialogueId">The dialogue ID.</param>
    public async Task Delete(Guid userId, Guid botId, Guid dialogueId)
    {
        var dialogue = await unitOfWork
            .GetRepository<Dialogue>()
            .GetFirstOrDefaultAsync(
                disableTracking: false,
                predicate: x =>
                    x.CopilotChatBotId == botId
                    && x.CopilotChatBot.UserId == userId
                    && x.Id == dialogueId,
                include: x => x.Include(d => d.CopilotChatBot)
            );

        if (dialogue is null)
        {
            return;
        }

        unitOfWork.GetRepository<Dialogue>().Delete(dialogue);
        await unitOfWork.SaveChangesAsync();

        await assistantRepository.DeleteThreadAsync(dialogue.CopilotChatBot.AssistantId, dialogue.ThreadId);
    }
}