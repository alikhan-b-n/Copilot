using Microsoft.AspNetCore.Identity;

namespace Copilot.Infrastructure.Entities;

public class User : IdentityUser<Guid>
{
    public virtual ICollection<CopilotChatBot> CopilotChatBots { get; set; }
}