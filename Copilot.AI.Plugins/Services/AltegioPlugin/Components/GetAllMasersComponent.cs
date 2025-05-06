using System.Collections;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models.EmployeeService;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

/// <summary>
/// Component for retrieving all masters from Altegio.
/// </summary>
public class GetAllMasersComponent : IAltegioComponent
{
    private readonly IAltegioApiBuilder _altegioApiBuilder;

    public GetAllMasersComponent(IAltegioApiBuilder altegioApiBuilder)
    {
        _altegioApiBuilder = altegioApiBuilder;
    }

    public Type ResultType => typeof(List<EmployeeResponse>);


    public static readonly string ExternalCompanyIdParameterKey = "companyId";

    /// <summary>
    /// Executes the Altegio component to retrieve all masters from Altegio.
    /// </summary>
    /// <param name="hashtable">The parameters required for the component execution.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the component execution.</returns>
    /// <exception cref="ArgumentException">Thrown when the companyId parameter is missing.</exception>
    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        var companyId = hashtable[ExternalCompanyIdParameterKey] as int? ??
                        throw new ArgumentException("No companyId parameter");

        try
        {
            var altegioApi = await _altegioApiBuilder.BuildByExternalCompanyIdAsync(companyId);

            var employeeResponse =
                await altegioApi.EmployeeService.GetEmployeesAsync(companyId);

            return new AltegioComponentResult
            {
                IsSuccess = true,
                Result = employeeResponse.Data.Where(x => x.Fired == 0).ToList() // Not Fired,
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