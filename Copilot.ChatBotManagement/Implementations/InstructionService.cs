using Calabonga.UnitOfWork;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Copilot.ChatBotManagement.Implementations;

public class InstructionService(IUnitOfWork<ApplicationContext> unitOfWork) : IInstructionFileService
{
    public async Task<InstructionFileResponse> CreateInstructionFile(AddInstructionFile addInstructionFile)
    {
        var fileEntity = new InstructionFile
        {
            BotId = addInstructionFile.BotId,
            UserId = addInstructionFile.UserId,
            FileName = addInstructionFile.FileName,
            Url = "temporary string"
        };

        await unitOfWork.GetRepository<InstructionFile>().InsertAsync(fileEntity);
        await unitOfWork.SaveChangesAsync();
        var instructionFile = await unitOfWork
            .GetRepository<InstructionFile>()
            .GetAll(disableTracking: true)
            .Where(x => x.UserId == fileEntity.UserId && x.Id == fileEntity.Id)
            .FirstOrDefaultAsync();

        if (instructionFile == null)
            throw new ArgumentException($"{nameof(instructionFile)} was not found", nameof(instructionFile));

        return MapToResponse(instructionFile);
    }

    public async Task<InstructionFileResponse> GetInstructionFileByBotId(Guid userId, Guid id)
    {
        var instructionFile = await unitOfWork
            .GetRepository<InstructionFile>()
            .GetAll(disableTracking: true)
            .Where(x => x.UserId == userId && x.Id == id)
            .FirstOrDefaultAsync();

        if (instructionFile == null)
            throw new ArgumentException($"{nameof(instructionFile)} was not found", nameof(instructionFile));

        return MapToResponse(instructionFile);
    }

    public async Task DeleteInstructionFile(Guid userId, Guid id)
    {
        var instructionFile = await unitOfWork
            .GetRepository<InstructionFile>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == id && x.UserId == userId);

        if (instructionFile == null)
            throw new ArgumentException(nameof(instructionFile), $"{nameof(instructionFile)} was not found");

        unitOfWork.GetRepository<InstructionFile>().Delete(instructionFile.Id);
        await unitOfWork.SaveChangesAsync();
    }

    private InstructionFileResponse MapToResponse(InstructionFile instruction)
    {
        return new InstructionFileResponse
        {
            Id = instruction.Id,
            UserId = instruction.UserId,
            Url = instruction.Url,
            BotId = instruction.BotId
        };
    }
}