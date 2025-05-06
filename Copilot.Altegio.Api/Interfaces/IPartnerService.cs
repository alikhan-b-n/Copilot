using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.PartnerService;

namespace Copilot.Altegio.Api.Interfaces;

public interface IPartnerService
{
    Task<AltegioSingleResponse<ConnectionStatusResponse>> ConnectionStatusAsync(int salonId, int applicationId);
}