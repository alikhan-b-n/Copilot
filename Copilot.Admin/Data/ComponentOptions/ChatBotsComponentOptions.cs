using Copilot.Contracts.Parameters;

namespace Copilot.Admin.Data.ComponentOptions;

public class ChatBotsComponentOptions : ComponentOption<CopilotChatBotRequest>
{
    public ChatBotsComponentOptions()
    {
        Context = new CopilotChatBotRequest();
    }
}