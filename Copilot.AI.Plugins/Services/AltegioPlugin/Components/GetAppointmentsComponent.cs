using System.Collections;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models.RecordService;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

public class GetAppointmentsComponent : IAltegioComponent
{
    private readonly IAltegioApiBuilder _altegioApiBuilder;

    public GetAppointmentsComponent(IAltegioApiBuilder altegioApiBuilder)
    {
        _altegioApiBuilder = altegioApiBuilder;
    }

    public Type ResultType => typeof(List<RecordResponse>);

    public static readonly string AltegioCompanyIdParameterKey = "companyId";
    public static readonly string PhoneNumberParameterKey = "phoneNumber";

    /// <summary>
    /// Executes the Altegio component to retrieve all masters from Altegio.
    /// </summary>
    /// <param name="hashtable">The parameters required for the component execution.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the component execution.</returns>
    /// <exception cref="ArgumentException">Thrown when the companyId parameter is missing.</exception>
    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        var companyId = hashtable[AltegioCompanyIdParameterKey] as int? ??
                        throw new ArgumentException("No companyId parameter");

        var phoneNumber = hashtable[PhoneNumberParameterKey] as string ??
                          throw new ArgumentException("No phoneNumber parameter");

        try
        {
            var altegioApi = await _altegioApiBuilder.BuildByExternalCompanyIdAsync(companyId);

            var clientId = await GetClientId(phoneNumber, companyId, altegioApi);
            
            return await GetRecords(clientId, companyId, altegioApi);
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

    private static async Task<AltegioComponentResult> GetRecords(int clientId, int companyId, IAltegioAPI altegioApi)
    {
        var records =
            await altegioApi.RecordService.GetRecordsAsync(companyId, clientId: clientId);

        if (!records.Success)
        {
            return new AltegioComponentResult
            {
                IsSuccess = false,
                ErrorMessage = "Failed to find active appointments."
            };
        }

        return new AltegioComponentResult
        {
            IsSuccess = true,
            Result = records.Data.Where(x => x.Attendance is (int)Attendance.Waiting or (int)Attendance.Confirmed)
                .ToList()
        };
    }

    private async Task<int> GetClientId(string phoneNumber, int companyId, IAltegioAPI altegioApi)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ExpectedException("To get this data I need your phone number");
        
        var errorException = new ExpectedException($"No data found on phoneNumber:{phoneNumber}");
        
        var clientsResponse = await altegioApi.ClientService.GetClientsAsync(companyId, phone: phoneNumber);
        if (!clientsResponse.Success || !clientsResponse.Data.Any()) throw errorException;

        var client = clientsResponse.Data.FirstOrDefault();
        return client?.Id ?? throw errorException;
    }
}