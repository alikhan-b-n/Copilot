using System.Collections;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.Altegio.Api.Interfaces;
using Copilot.Altegio.Api.Models.RecordService;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

public class TransferAppointmentComponent : IAltegioComponent
{
    private readonly IAltegioApiBuilder _altegioApiBuilder;

    public TransferAppointmentComponent(IAltegioApiBuilder altegioApiBuilder)
    {
        _altegioApiBuilder = altegioApiBuilder;
    }

    public Type ResultType { get; } = typeof(bool);

    public const string ExternalCompanyIdParameterKey = "companyId";
    public const string RecordIdParameterKey = "recordId";
    public const string DateParameterKey = "date";

    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        var companyId = hashtable[ExternalCompanyIdParameterKey] as int? ??
                        throw new ArgumentException("No companyId parameter");
        var recordId = hashtable[RecordIdParameterKey] as int? ??
                       throw new ArgumentException("No recordId parameter");
        var date = hashtable[DateParameterKey] as DateTimeOffset? ??
                   throw new ArgumentException("No date parameter");

        try
        {
            var altegioApi = await _altegioApiBuilder.BuildByExternalCompanyIdAsync(companyId);

            var record = await altegioApi.RecordService.GetRecordAsync(companyId, recordId);
            if (record is not { Success: true })
            {
                return new AltegioComponentResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to transfer appointment. " + record.Meta
                };
            }

            var result = await altegioApi.RecordService.EditRecordAsync(companyId, recordId,
                new RecordPut
                {
                    Datetime = date.ToString(),
                    Services = record.Data.Services.Select(s => new RecordPutService { Id = s.Id }),
                    SeanceLength = record.Data.SeanceLength,
                    StaffId = record.Data.StaffId,
                    Client = new RecordPutClient
                    {
                        Id = record.Data.Client.Id
                    }
                });

            if (!result.Success)
            {
                return new AltegioComponentResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to transfer appointment. " + result.Meta
                };
            }
        }
        catch (Exception e)
        {
            return new AltegioComponentResult
            {
                IsSuccess = false,
                ErrorMessage = "Failed to transfer appointment. " + e.Message
            };
        }

        return new AltegioComponentResult
        {
            IsSuccess = true,
            Result = true
        };
    }
}
