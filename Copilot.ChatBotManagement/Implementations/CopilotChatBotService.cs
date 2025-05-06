using Calabonga.UnitOfWork;
using Copilot.Ai.Interfaces;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Copilot.ChatBotManagement.Implementations;

public class CopilotChatBotService(IUnitOfWork<ApplicationContext> unitOfWork, IAssistantRepository assistantRepository)
    : ICopilotChatBotService
{
    public async Task<List<CopilotChatBotResponse>> GetAllChatBots(Guid userId)
    {
        var chatBots = await unitOfWork
            .GetRepository<CopilotChatBot>()
            .GetAll(disableTracking: true)
            .Where(x => x.UserId == userId)
            .Select(x => new CopilotChatBotResponse
            {
                Id = x.Id,
                Title = x.Title,
                Status = (int)x.Status,
                PersonalityPrompt = x.PersonalityPrompt,
                Timezone = x.Timezone,
                GptModelId = x.GptModelId,
                UserId = userId
            })
            .ToListAsync();

        return chatBots;
    }

    public async Task<CopilotChatBotResponse> GetChatBotById(Guid userId, Guid id)
    {
        var chatBot = await unitOfWork
            .GetRepository<CopilotChatBot>()
            .GetAll(disableTracking: true)
            .Where(x => x.UserId == userId && x.Id == id)
            .Select(x => new CopilotChatBotResponse
            {
                Id = x.Id,
                Title = x.Title,
                Status = (int)x.Status,
                PersonalityPrompt = x.PersonalityPrompt,
                Timezone = x.Timezone,
                GptModelId = x.GptModelId,
                UserId = x.UserId
            })
            .FirstOrDefaultAsync();

        return chatBot ?? throw new ArgumentException($"Chat bot is not found. UserId: {userId}, botId: {id}");
    }

    public async Task<CopilotChatBotResponse> CreateChatBot(Guid userId, CopilotChatBotRequest chatBotRequest)
    {
        var gptModel = await unitOfWork
            .GetRepository<GptModel>()
            .GetAll(disableTracking: true)
            .Where(x => x.Id ==
                        chatBotRequest.GptModelId).FirstAsync();

        var assistant = await assistantRepository.CreateAsync(gptModel.Name);

        var chatBotEntity = new CopilotChatBot
        {
            Title = chatBotRequest.Title,
            AssistantId = assistant.Id,
            Status = (ChatBotStatus)chatBotRequest.Status,
            PersonalityPrompt = chatBotRequest.PersonalityPrompt,
            Timezone = 0,
            GptModelId = chatBotRequest.GptModelId,
            UserId = userId
        };

        await unitOfWork.GetRepository<CopilotChatBot>().InsertAsync(chatBotEntity);
        await unitOfWork.SaveChangesAsync();

        return MapToResponse(chatBotEntity);
    }

    public async Task<CopilotChatBotResponse> UpdateChatBot(Guid id, Guid userId, CopilotChatBotUpdateRequest chatBot)
    {
        var existingChatBot = await unitOfWork
            .GetRepository<CopilotChatBot>()
            .GetFirstOrDefaultAsync(
                predicate: x => x.Id == id && x.UserId == userId,
                disableTracking: false
            );

        if (existingChatBot is null)
            throw new ArgumentNullException(nameof(existingChatBot));

        var gptModel = await unitOfWork
            .GetRepository<GptModel>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == chatBot.GptModelId);

        if (gptModel is null)
            throw new ArgumentNullException(nameof(gptModel));

        existingChatBot.Title = chatBot.Title ?? existingChatBot.Title;
        existingChatBot.Status = (ChatBotStatus)(chatBot.Status ?? (int)existingChatBot.Status);
        existingChatBot.PersonalityPrompt = chatBot.PersonalityPrompt ?? existingChatBot.PersonalityPrompt;
        existingChatBot.Timezone = chatBot.Timezone ?? existingChatBot.Timezone;
        existingChatBot.GptModelId = gptModel.Id;

        await assistantRepository.UpdateAsync(
            assistantId: existingChatBot.AssistantId,
            gptModel: gptModel.Name
        );

        await unitOfWork.SaveChangesAsync();

        return MapToResponse(existingChatBot);
    }

    public async Task DeleteChatBot(Guid id, Guid userId)
    {
        var chatBot = await unitOfWork
            .GetRepository<CopilotChatBot>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == id && x.UserId == userId);

        if (chatBot == null)
            throw new ArgumentException(nameof(id));

        await assistantRepository.DeleteAsync(chatBot.AssistantId);
        unitOfWork.GetRepository<CopilotChatBot>().Delete(chatBot.Id);

        await unitOfWork.SaveChangesAsync();
    }

    private CopilotChatBotResponse MapToResponse(CopilotChatBot chatBotEntity)
    {
        return new CopilotChatBotResponse
        {
            Id = chatBotEntity.Id,
            Title = chatBotEntity.Title,
            Status = (int)chatBotEntity.Status,
            PersonalityPrompt = chatBotEntity.PersonalityPrompt,
            Timezone = chatBotEntity.Timezone,
            GptModelId = chatBotEntity.GptModelId,
            UserId = chatBotEntity.UserId
        };
    }
}