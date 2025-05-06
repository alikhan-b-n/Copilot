using Microsoft.AspNetCore.Http;

namespace Copilot.Contracts.Parameters;

public class AddFileAsKnowledgeRequestModel
{
    public Guid BotId { get; set; }
    public IFormFile Content { get; set; } = null!;
}