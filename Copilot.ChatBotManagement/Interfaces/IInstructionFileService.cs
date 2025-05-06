using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;

namespace Copilot.ChatBotManagement.Interfaces;

public interface IInstructionFileService
{
    Task<InstructionFileResponse> CreateInstructionFile(AddInstructionFile addInstructionFile);
    Task<InstructionFileResponse> GetInstructionFileByBotId(Guid userId, Guid id);
    Task DeleteInstructionFile(Guid userId, Guid id);
}
