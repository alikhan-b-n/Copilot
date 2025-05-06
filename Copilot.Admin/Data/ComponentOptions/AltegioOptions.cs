using Copilot.Admin.Data.Apis;

namespace Copilot.Admin.Data.ComponentOptions;

public class AltegioOptions : ComponentOption<CreateAltegioCompanyParams>
{
    public AltegioOptions()
    {
        Context = new CreateAltegioCompanyParams();
    }
}