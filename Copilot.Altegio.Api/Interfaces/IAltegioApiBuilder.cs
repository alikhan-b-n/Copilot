namespace Copilot.Altegio.Api.Interfaces;

public interface IAltegioApiBuilder
{
    Task<IAltegioAPI> BuildByExternalCompanyIdAsync(int externalCompanyId);
}