using Copilot.Admin.Data.Apis;

namespace Copilot.Admin.Data.ComponentOptions;

public class WhatsAppComponentOptions : ComponentOption<CreateWhatsAppAccountParams>
{
    public WhatsAppComponentOptions()
    {
        Context = new CreateWhatsAppAccountParams();
    }
}