using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.AI.Plugins.Services.AltegioPlugin.Helpers;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models.BookformService;
using Microsoft.Extensions.Caching.Distributed;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

/// <summary>
/// Retrieves a list of services from the YClients API.
/// </summary>
public class GetAllServicesComponent : IAltegioComponent
{
    private readonly IAltegioApiBuilder _yclientsApiBuilder;
    private readonly IDistributedCache _cache;

    public GetAllServicesComponent(IDistributedCache cache, IAltegioApiBuilder yclientsApiBuilder)
    {
        _cache = cache;
        _yclientsApiBuilder = yclientsApiBuilder;
    }

    public Type ResultType => typeof(List<BookService>);
    
    public static readonly string YClientsCompanyIdParameterKey = "companyId";
    public static readonly string MasterIdParameterKey = "masterId";

    /// <summary>
    /// Executes the Invoke method of the GetAllServicesComponent class.
    /// </summary>
    /// <param name="hashtable">
    /// The input parameters for the Invoke method.
    /// Hashtable must contain the following optional parameters:
    /// - "companyId": int? (The company ID)
    /// - "serviceIds": int[] (Ids of allowed services)
    /// - "masterId": int? (The master ID)
    /// </param>
    /// <returns>
    /// Returns a Task of YClientsComponentResult.
    /// The YClientsComponentResult object contains the following properties:
    /// - IsSuccess: bool (Indicates whether the Invoke method was successful or not)
    /// - Result: object (The result of the Invoke method)
    /// - ErrorMessage: string? (The error message, if any)
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the "companyId" parameter or the "masterId" parameter is missing.
    /// </exception>
    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        var companyId = hashtable[YClientsCompanyIdParameterKey] as int? ?? throw new ArgumentException("No companyId parameter");
        var masterId = hashtable[MasterIdParameterKey] as int? ?? throw new ArgumentException("No masterId parameter");

        var cacheKey = AltegioPluginComponentsCacheKeys.ServicesOfMasterKey
            .Replace("{companyId}", companyId.ToString())
            .Replace("{masterId}", masterId.ToString());
        
        try
        {
            var data = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrWhiteSpace(data))
            {
                return new AltegioComponentResult
                {
                    IsSuccess = true,
                    Result = JsonSerializer.Deserialize<List<BookService>>(data)
                };
            }

            var services = await LoadServices(companyId, masterId);

            var json = JsonSerializer.Serialize(services, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            
            await _cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3)
            });
            
            return new AltegioComponentResult
            {
                IsSuccess = true,
                Result = services
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

    /// <summary>
    /// Retrieves a list of services from the YClients API.
    /// </summary>
    /// <param name="companyId">The ID of the company.</param>
    /// <param name="masterId">The ID of the master.</param>
    /// <returns>The list of book services.</returns>
    private async Task<List<BookService>> LoadServices(int companyId, int masterId)
    {
        var yClientsApi = await _yclientsApiBuilder.BuildByExternalCompanyIdAsync(companyId);
        var bookServicesResponse = await yClientsApi.BookformService.GetBookServicesAsync(companyId, masterId);

        var result = bookServicesResponse.Data.Services;

        return result;
    }
}