using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.ServicesService;

namespace Copilot.Altegio.Api.Interfaces;

/// <summary>
/// Represents a service that handles operations related to services in Copilot Altegio API.
/// </summary>
public interface IServicesService
{
    Task<AltegioResponse<ServiceCategoryResponse>> GetServiceCategoriesAsync(int companyId,
        int? serviceCategoryId = null);
}
