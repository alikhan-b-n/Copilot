using System.Collections;
using Copilot.AI.Plugins.Services.AltegioPlugin.Components;
using Copilot.Altegio.Api.Models.BookformService;
using Copilot.Altegio.Api.Models.EmployeeService;
using Copilot.Altegio.Api.Models.ServicesService;

namespace Copilot.AI.Plugins.Services.AltegioPlugin;

public partial class AltegioPluginService
{
    /// <summary>
    /// Gets masters matching the specified query.
    /// </summary>
    /// <param name="companyId">The ID of the company.</param>
    /// <param name="masterQuery">The query used to filter masters.</param>
    /// <returns>List of tuples containing the ID and additional information of the masters.</returns>
    private async Task<List<(int Id, string Info)>> GetMastersByQuery(int companyId, string masterQuery)
    {
        var getAllMasterComponentResult = await _components[nameof(GetAllMasersComponent)].InvokeAsync(new Hashtable
        {
            {
                GetAllMasersComponent.ExternalCompanyIdParameterKey, companyId
            }
        });

        var allMasters = (getAllMasterComponentResult.OrThrow().Result as List<EmployeeResponse>)!
            .Select(x => new { x.Id, x.Name, x.Specialization })
            .ToList();

        var getMasterIdsByQueryComponentResult = await _components[nameof(GetIdsByQueryComponent)]
            .InvokeAsync(
                new Hashtable
                {
                    { GetIdsByQueryComponent.QueryKey, masterQuery },
                    {
                        GetIdsByQueryComponent.DataSetKey,
                        allMasters.Select(x => (x.Id, Info: $"[{x.Id}] {x.Name} - {x.Specialization}")).ToList()
                    }
                });

        return getMasterIdsByQueryComponentResult.OrThrow().Result as List<(int Id, string Info)> ?? [];
    }
    
    /// <summary>
    /// Retrieves a list of services based on the provided query.
    /// </summary>
    /// <param name="serviceQuery">The query string used to search for services.</param>
    /// <param name="companyId">The ID of the company.</param>
    /// <param name="masterId">The ID of the master.</param>
    /// <returns>A list of services along with their categories.</returns>
    private async Task<List<(BookService Service, string Category)>> GetServicesByQuery(
        string serviceQuery, int companyId, int masterId)
    {
        // GetAllCategories
        var getAllCategoriesResult = await _components[nameof(GetAllCategoriesComponent)].InvokeAsync(new Hashtable
        {
            {
                GetAllCategoriesComponent.YClientsCompanyIdParameterKey, companyId
            }
        });

        var categories = (getAllCategoriesResult.OrThrow().Result as List<ServiceCategoryResponse>)!;

        // GetAllServices

        var getAllServicesComponentResult = await _components[nameof(GetAllServicesComponent)].InvokeAsync(new Hashtable
        {
            { GetAllServicesComponent.YClientsCompanyIdParameterKey, companyId },
            { GetAllServicesComponent.MasterIdParameterKey, masterId },
        });

        var allServices = (getAllServicesComponentResult.OrThrow().Result as List<BookService>)!;

        var servicesWithCategory = allServices
            .Select(x => (Service: x, Category: categories.FirstOrDefault(c => c.Id == x.CategoryId)))
            .ToList();

        // GetServiceIdByQuery
        var getServiceIdsByQueryComponentResult = await _components[nameof(GetIdsByQueryComponent)].InvokeAsync(
            new Hashtable
            {
                { GetIdsByQueryComponent.QueryKey, serviceQuery },
                {
                    GetIdsByQueryComponent.DataSetKey,
                    servicesWithCategory
                        .Select(x => (x.Service.Id, Info: $"[{x.Service.Id}] {x.Service.Title} - {x.Category?.Title}"))
                        .ToList()
                }
            });

        var found = getServiceIdsByQueryComponentResult.OrThrow().Result as List<(int Id, string Info)>
                    ?? new List<(int Id, string Info)>();

        return servicesWithCategory
            .Where(x => found.Exists(f => f.Id == x.Service.Id))
            .Select(x => (x.Service, x.Category?.Title ?? string.Empty))
            .ToList();
    }
}