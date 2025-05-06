using System.Collections;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.Altegio.Api.Interfaces;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

public class CancelAppointmentComponent : IAltegioComponent
{
    private record CancelAppointmentComponentResult(int Id, bool IsSuccess, string ErrorMessage);
    private readonly IAltegioApiBuilder _AltegioApiBuilder;
    public CancelAppointmentComponent(IAltegioApiBuilder AltegioApiBuilder)
    {
        _AltegioApiBuilder = AltegioApiBuilder;
    }

    public Type ResultType { get; } = typeof(List<CancelAppointmentComponentResult>);

    public const string AltegioCompanyIdParameterKey = "companyId";
    public const string RecordIdsParameterKey = "recordIds";

    /// <summary>
    ///  Invokes the CancelAppointmentComponent.
    /// </summary>
    /// <param name="hashtable">The hashtable containing the required parameters.</param>
    /// <returns>A Task of AltegioComponentResult.</returns>
    /// <exception cref="ArgumentException">Thrown when the required parameters are not provided.</exception>
    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        await Task.CompletedTask;
        var companyId = hashtable[AltegioCompanyIdParameterKey] as int? ??
                        throw new ArgumentException("No companyId parameter");
        var recordIds = hashtable[RecordIdsParameterKey] as List<int> ??
                        throw new ArgumentException("No recordIds parameter");

        var results = new List<CancelAppointmentComponentResult>();

        try
        {
            var altegioApi = await _AltegioApiBuilder.BuildByExternalCompanyIdAsync(companyId);

            foreach (var recordId in recordIds)
            {
                await altegioApi.RecordService.DeleteRecordAsync(companyId, recordId);
                results.Add(new CancelAppointmentComponentResult(recordId, true, string.Empty));
            }
        }
        catch (Exception e)
        {
            return new AltegioComponentResult
            {
                IsSuccess = false,
                ErrorMessage = "Failed to cancel appointment. " + e.Message
            };
        }

        return new AltegioComponentResult
        {
            IsSuccess = true,
            Result = results
        };
    }
}
