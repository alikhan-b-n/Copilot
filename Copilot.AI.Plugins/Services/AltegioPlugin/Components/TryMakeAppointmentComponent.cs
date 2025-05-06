using System.Collections;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models.BookformService;
using Microsoft.Extensions.Logging;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

/// <summary>
/// Represents a component used for making appointments using the Altegio API.
/// </summary>
public class TryMakeAppointmentComponent : IAltegioComponent
{
    private readonly IAltegioApiBuilder _altegioApiBuilder;
    private readonly ILogger<TryMakeAppointmentComponent> _logger;
    
    public TryMakeAppointmentComponent(IAltegioApiBuilder altegioApiBuilder,
        ILogger<TryMakeAppointmentComponent> logger)
    {
        _altegioApiBuilder = altegioApiBuilder;
        _logger = logger;
    }

    public Type ResultType => typeof(bool);

    public static readonly string ExternalCompanyIdParameterKey = "companyId";
    public static readonly string MasterIdParameterKey = "masterId";
    public static readonly string DateParameterKey = "date";
    public static readonly string ServiceIdParameterKey = "serviceId";

    /// <summary>
    /// Invokes the TryMakeAppointmentComponent to make appointments using the Altegio API.
    /// </summary>
    /// <param name="hashtable">The hashtable containing the parameters required for making the appointment.</param>
    /// <returns>Returns a Task of AltegioComponentResult.</returns>
    /// <exception cref="ArgumentException">Throws an exception if any of the required parameters are missing.</exception>
    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        var companyId = hashtable[ExternalCompanyIdParameterKey] as int? ??
                        throw new ArgumentException("No companyId parameter");
        var masterId = hashtable[MasterIdParameterKey] as int? ?? throw new ArgumentException("No masterId parameter");
        var date = hashtable[DateParameterKey] as DateTimeOffset? ?? throw new ArgumentException("No date parameter");
        var serviceId = hashtable[ServiceIdParameterKey] as int? ??
                        throw new ArgumentException("No serviceId parameter");

        try
        {
            var altegioApi = await _altegioApiBuilder.BuildByExternalCompanyIdAsync(companyId);
            var bookCheck = new BookCheckParameter
            {
                Appointments = new List<AppointmentParameter>
                {
                    new()
                    {
                        Id = 777,
                        Services = new List<int> { serviceId },
                        StaffId = masterId,
                        Datetime = date.ToString("yyyyMMdd HH:mm")
                    }
                }
            };

            _logger.LogInformation("Request TryMakeAppointmentComponent {@bookCheck}", bookCheck);

            var result = await altegioApi.BookformService.BookCheck(companyId, bookCheck);

            if (result.Success)
            {
                return new AltegioComponentResult
                {
                    IsSuccess = true,
                    Result = result.Success,
                };
            }

            if (result.Meta.Errors is null)
            {
                return new AltegioComponentResult
                {
                    IsSuccess = result.Success,
                    ErrorMessage = result.Meta.Message
                };
            }

            var i = 1;
            var errors = string.Join("\n", result.Meta.Errors.Select(x => $"{i++}. {x.Message}."));

            return new AltegioComponentResult
            {
                IsSuccess = result.Success,
                ErrorMessage = errors
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