using System.Collections;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models.ServicesService;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

public class GetAllCategoriesComponent : IAltegioComponent
{
    private readonly IAltegioApiBuilder _altegioApiBuilder;

    public GetAllCategoriesComponent(IAltegioApiBuilder altegioApiBuilder)
    {
        _altegioApiBuilder = altegioApiBuilder;
    }

    public Type ResultType => typeof(List<ServiceCategoryResponse>);


    public static readonly string YClientsCompanyIdParameterKey = "companyId";

    /// <summary>
    /// Asynchronously invokes the method using the specified hashtable as the parameter.
    /// </summary>
    /// <param name="hashtable">The hashtable parameter.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the companyId parameter is not provided in the hashtable.</exception>
    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        var companyId = hashtable[YClientsCompanyIdParameterKey] as int? ??
                        throw new ArgumentException("No companyId parameter");
        
        try
        {
            var altegioApi = await _altegioApiBuilder.BuildByExternalCompanyIdAsync(companyId);

            var categoriesResponse =
                await altegioApi.ServicesService.GetServiceCategoriesAsync(companyId);

            return new AltegioComponentResult
            {
                IsSuccess = true,
                Result = categoriesResponse.Data.ToList()
            };
        }
        catch (Exception e)
        {
            return new AltegioComponentResult
            {
                IsSuccess = false,
                ErrorMessage = e.Message
            };
        }
    }
}