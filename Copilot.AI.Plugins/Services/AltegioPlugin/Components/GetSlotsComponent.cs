using System.Collections;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models.BookformService;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

/// <summary>
/// Represents a component that retrieves available slots for booking from the Altegio API.
/// </summary>
public class GetSlotsComponent : IAltegioComponent
{
    private readonly IAltegioApiBuilder _altegioApiBuilder;

    public GetSlotsComponent(IAltegioApiBuilder altegioApiBuilder)
    {
        _altegioApiBuilder = altegioApiBuilder;
    }

    public Type ResultType => typeof(List<SeanceResponse>);

    public static readonly string ExternalCompanyIdParameterKey = "companyId";
    public static readonly string MasterIdParameterKey = "masterId";
    public static readonly string DateParameterKey = "date";
    public static readonly string ServiceIdsParameterKey = "servicesIds";

    /// <summary>
    /// Executes the invocation of retrieving available slots for booking from the Altegio API.
    /// </summary>
    /// <param name="hashtable">A hashtable containing the required parameters for the retrieval.</param>
    /// <returns>A task representing the asynchronous operation. The result contains a AltegioComponentResult object which indicates the success of the operation, the retrieved slots, and any error message if applicable.</returns>
    /// <exception cref="ArgumentException">Thrown when one or more required parameters are missing.</exception>
    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        var companyId = hashtable[ExternalCompanyIdParameterKey] as int? ??
                        throw new ArgumentException("No companyId parameter");
        var masterId = hashtable[MasterIdParameterKey] as int? ?? throw new ArgumentException("No masterId parameter");
        var date = hashtable[DateParameterKey] as DateTimeOffset?;
        var servicesIds = hashtable[ServiceIdsParameterKey] as int[] ??
                          throw new ArgumentException("No servicesIds parameter");

        try
        {
            var altegioApi = await _altegioApiBuilder.BuildByExternalCompanyIdAsync(companyId);

            var freeSlots =
                await GetBookTimes(altegioApi, companyId, masterId, date, servicesIds) ??
                (
                    masterId == 0
                        ? await GetBookTimes(altegioApi, companyId, masterId, DateTimeOffset.Now, servicesIds)
                        : await GetBookStuffSeances(altegioApi, companyId, masterId, date, servicesIds)
                );

            return new AltegioComponentResult
            {
                IsSuccess = true,
                Result = freeSlots
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

    private async Task<List<SeanceResponse>?> GetBookTimes(IAltegioAPI altegioApi,
        int companyId, int masterId, DateTimeOffset? date, int[]? servicesIds)
    {
        try
        {
            if (!date.HasValue) return null!;

            var result = await altegioApi.BookformService.GetBookTimes(
                companyId, masterId, date.Value, servicesIds
            );

            if (!result.Success || !result.Data.Any())
            {
                return null!;
            }

            return result.Data.ToList();
        }
        catch (Exception)
        {
            return null!;
        }
    }

    private async Task<List<SeanceResponse>?> GetBookStuffSeances(IAltegioAPI altegioApi,
        int companyId, int masterId, DateTimeOffset? date, int[]? servicesIds)
    {
        var result = await altegioApi.BookformService.GetBookStuffSeances(
            companyId, masterId, date, servicesIds
        );

        return result.Data.Seances.ToList();
    }
}