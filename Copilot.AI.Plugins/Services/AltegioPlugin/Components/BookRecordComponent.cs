using System.Collections;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models.BookformService;
using Microsoft.Extensions.Logging;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

public class BookRecordComponent : IAltegioComponent
{
    private readonly IAltegioApiBuilder _yclientsApiBuilder;
    private readonly ILogger<BookRecordComponent> _logger;

    public BookRecordComponent(IAltegioApiBuilder yclientsApiBuilder, ILogger<BookRecordComponent> logger)
    {
        _yclientsApiBuilder = yclientsApiBuilder;
        _logger = logger;
    }

    public Type ResultType => typeof(int);

    public static readonly string YClientsCompanyIdParameterKey = "companyId";
    public static readonly string MasterIdParameterKey = "masterId";
    public static readonly string DateParameterKey = "date";
    public static readonly string ServiceIdParameterKey = "serviceId";
    public static readonly string ClientNameKey = "clientName";
    public static readonly string ClientPhoneKey = "clientPhone";

    /// <summary>
    /// Invokes the TryMakeAppointmentComponent to make appointments using the YClients API.
    /// </summary>
    /// <param name="hashtable">The hashtable containing the parameters required for making the appointment.</param>
    /// <returns>Returns a Task of YClientsComponentResult.</returns>
    /// <exception cref="ArgumentException">Throws an exception if any of the required parameters are missing.</exception>
    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        var companyId = hashtable[YClientsCompanyIdParameterKey] as int? ??
                        throw new ArgumentException("No companyId parameter");
        var masterId = hashtable[MasterIdParameterKey] as int? ?? throw new ArgumentException("No masterId parameter");
        var date = hashtable[DateParameterKey] as DateTimeOffset? ?? throw new ArgumentException("No date parameter");
        var serviceId = hashtable[ServiceIdParameterKey] as int? ??
                        throw new ArgumentException("No serviceId parameter");
        var clientName = hashtable[ClientNameKey] as string ??
                        throw new ArgumentException("No clientName parameter");
        var clientPhone = hashtable[ClientPhoneKey] as string ??
                         throw new ArgumentException("No clientName parameter");

        try
        {
            var yclientsApi = await _yclientsApiBuilder.BuildByExternalCompanyIdAsync(companyId);

            var bookRecord = new BookRecordParameter
            {
                Phone = clientPhone,
                Fullname = clientName,
                Email = string.Empty,
                Comment = "Created by Copilot",
                Appointments =
                [
                    new()
                    {
                        Id = 1,
                        Services = new List<int> { serviceId },
                        StaffId = masterId,
                        Datetime = date.ToString("yyyyMMdd HH:mm")
                    }
                ]
            };
            
            _logger.LogInformation(message:"Request BookRecordComponent {@bookRecord}", bookRecord);
            
            var result = await yclientsApi.BookformService.BookRecord(companyId, bookRecord);
            
            return new AltegioComponentResult
            {
                IsSuccess = result.Success,
                Result = result.Data.FirstOrDefault()?.RecordId
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