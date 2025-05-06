using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using Calabonga.UnitOfWork;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.AI.Plugins.Services.AltegioPlugin.Components;
using Copilot.Altegio.Api.Models.BookformService;
using Copilot.Experimental.Agents.Extensions;
using Copilot.Infrastructure.Entities;
using Copilot.Utils;

namespace Copilot.AI.Plugins.Services.AltegioPlugin;

/// <summary>
/// Represents a service for creating records in the Altegio system.
/// </summary>
public partial class AltegioPluginService : IAltegioPluginService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Dictionary<string, IAltegioComponent> _components;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public AltegioPluginService(Dictionary<string, IAltegioComponent> components, IUnitOfWork unitOfWork)
    {
        _components = components;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Retrieves the slots available for booking based on the specified parameters.
    /// </summary>
    /// <param name="parameter">The parameter containing the search criteria for the slots.</param>
    /// <param name="responseBuilder"></param>
    /// <returns>A string representing the found slots and the data used for the search.</returns>
    public async Task GetSlots(GetSlotsParameter parameter, AgentResponseBuilder responseBuilder)
    {
        responseBuilder.AppendMessage("Result of GetSlot function:");

        // GetCompanyId
        var companyId = parameter.PluginParameter.CompanyId;

        // GetMasterIdsByQuery
        var foundMasters = await GetMastersByQuery(companyId, parameter.MasterQuery);

        var master = string.IsNullOrWhiteSpace(parameter.MasterQuery)
            ? (Id: 0, Info: "Any master")
            : foundMasters.Count switch
            {
                0 => throw new ExpectedException("No master with searched name"),
                1 => foundMasters.First(),
                _ => throw new ExpectedException(
                    "Which master you need?: " + JsonSerializer.Serialize(
                        value: foundMasters.Select(x => x.Info),
                        options: _jsonSerializerOptions
                    )
                )
            };

        responseBuilder
            .AppendMessage($"Found master: {master.Info}")
            .AppendInstructions($"masterId = {master.Id}")
            .AppendInstructions($"masterInfo = {master.Info}");
        
        // GetServiceIdByQuery
        var foundServices = await GetServicesByQuery(
            parameter.ServiceQuery,
            companyId,
            master.Id
        );

        var service = foundServices.Count switch
        {
            0 => throw new ExpectedException("No service with searched title"),
            1 => foundServices.First(),
            _ => throw new ExpectedException(
                "Which service do you need?: " + JsonSerializer.Serialize(
                    value: foundServices.Select(x => $"{x.Service.Title} ({x.Category})"),
                    options: _jsonSerializerOptions
                )
            )
        };

        responseBuilder
            .AppendMessage($"Found service: {service.Service.Title}")
            .AppendInstructions($"serviceId = {service.Service.Id}")
            .AppendInstructions($"serviceInfo = {service.Service.Title}");
        
        // GetDateByQuery
        var getDateByQueryComponentResult = await _components[nameof(GetDateByQueryComponent)].InvokeAsync(new Hashtable
        {
            { GetDateByQueryComponent.QueryParameterKey, parameter.WhenQuery },
            {
                GetDateByQueryComponent.TimezoneParameterKey,
                parameter.PluginParameter.Dialogue.CopilotChatBot.Timezone
            },
            { GetDateByQueryComponent.ConversationParameterKey, parameter.PluginParameter.Dialogue }
        });

        var date = (getDateByQueryComponentResult.OrThrow().Result as DateTimeOffset?)!;

        if (date.HasValue)
        {
            responseBuilder
                .AppendMessage($"Date in correct format: \'{date.Value:yyyyMMdd HH:mm}\'.")
                .AppendInstructions($"date = {date.Value:yyyyMMdd HH:mm}\'.");
        }

        // GetSlots
        var getSlotsComponentResult = await _components[nameof(GetSlotsComponent)].InvokeAsync(new Hashtable
        {
            { GetSlotsComponent.ExternalCompanyIdParameterKey, companyId },
            { GetSlotsComponent.MasterIdParameterKey, master.Id },
            { GetSlotsComponent.DateParameterKey, date },
            { GetSlotsComponent.ServiceIdsParameterKey, new[] { service.Service.Id } },
        });

        var slots = getSlotsComponentResult.OrThrow().Result as List<SeanceResponse>;

        if (DateTimeOffset.TryParse(slots?.FirstOrDefault()?.Datetime, out var slotsDate))
        {
            if (slotsDate.Date != date?.Date)
            {
                responseBuilder.AppendMessage("No slots available on requested date. " +
                                              "Here are the closest available slots.");
            }
        }

        responseBuilder
            .AppendMessage("Found slots date: " + slots?.FirstOrDefault()?.Datetime)
            .AppendMessage("Found slots: " + JsonSerializer.Serialize(slots, _jsonSerializerOptions));
    }

    public async Task CreateAppointment(CreateAppointmentParameter parameter, AgentResponseBuilder responseBuilder)
    {
        if (parameter.ServiceId == 0)
        {
            responseBuilder.AppendInstructions("serviceId cannot be 0, ask client about service one more time");
            return;
        }

        responseBuilder.AppendMessage("Result of CreateAppointment function:");

        // GetCompanyId
        var companyId = parameter.PluginParameter.CompanyId;

        // GetDateByQuery
        var getDateByQueryComponentResult = await _components[nameof(GetDateByQueryComponent)].InvokeAsync(new Hashtable
        {
            { GetDateByQueryComponent.QueryParameterKey, $"{parameter.DateSlot} {parameter.TimeSlot}" },
            {
                GetDateByQueryComponent.TimezoneParameterKey,
                parameter.PluginParameter.Dialogue.CopilotChatBot.Timezone
            },
            { GetDateByQueryComponent.ConversationParameterKey, parameter.PluginParameter.Dialogue }
        });

        var date = (getDateByQueryComponentResult.OrThrow().Result as DateTimeOffset?)!;

        if (!date.HasValue)
        {
            responseBuilder.AppendMessage("Failed to understand date");
            return;
        }

        // TryMakeAppointment
        var tryMakeAppointmentComponent = await _components[nameof(TryMakeAppointmentComponent)]
            .InvokeAsync(new Hashtable
            {
                { TryMakeAppointmentComponent.ExternalCompanyIdParameterKey, companyId },
                { TryMakeAppointmentComponent.MasterIdParameterKey, parameter.MasterId },
                { TryMakeAppointmentComponent.ServiceIdParameterKey, parameter.ServiceId },
                { TryMakeAppointmentComponent.DateParameterKey, date }
            });

        if (!tryMakeAppointmentComponent.IsSuccess)
        {
            responseBuilder.AppendMessage(tryMakeAppointmentComponent.ErrorMessage ?? string.Empty);
            return;
        }

        // Check Name and PhoneNumber
        if (string.IsNullOrWhiteSpace(parameter.ClientName))
        {
            responseBuilder.AppendMessage("To finish appointment creation I need your fullname.");
            return;
        }

        var phone = string.IsNullOrWhiteSpace(parameter.ClientPhone)
            ? parameter.PluginParameter.Dialogue.PhoneNumber
            : parameter.ClientPhone;

        if (string.IsNullOrWhiteSpace(phone) || phone.Length < 7)
        {
            responseBuilder.AppendMessage("To finish appointment creation I need your phone number.");
            return;
        }

        var bookRecordComponent = await _components[nameof(BookRecordComponent)]
            .InvokeAsync(new Hashtable
            {
                { BookRecordComponent.YClientsCompanyIdParameterKey, companyId },
                { BookRecordComponent.MasterIdParameterKey, parameter.MasterId },
                { BookRecordComponent.ServiceIdParameterKey, parameter.ServiceId },
                { BookRecordComponent.DateParameterKey, date },
                { BookRecordComponent.ClientNameKey, parameter.ClientName },
                { BookRecordComponent.ClientPhoneKey, phone },
            });

        if (!bookRecordComponent.IsSuccess)
        {
            responseBuilder.AppendMessage("Failed to make appointment, because: " +
                                          bookRecordComponent.ErrorMessage);
            return;
        }
        
        responseBuilder.AppendMessage("Successfully created appointment. Id: " + bookRecordComponent.Result);
    }

    public async Task GetMasterInformation(GetMasterInformationParameter parameter,
        AgentResponseBuilder responseBuilder)
    {
        responseBuilder.AppendMessage("Result of GetMasterInformation function:");

        // GetCompanyId
        var companyId = parameter.PluginParameter.CompanyId;

        // GetMasterIdsByQuery
        var foundMasters = await GetMastersByQuery(companyId, parameter.MasterQuery);

        if (foundMasters.Count == 0)
        {
            responseBuilder.AppendMessage("Could not find masters by query");
            return;
        }

        responseBuilder.AppendMessage("Found masters: " + JsonSerializer.Serialize(
            foundMasters.Select(x => new { x.Id, x.Info }),
            _jsonSerializerOptions));
    }

    public async Task GetServiceInformation(GetServiceInformationParameter parameter,
        AgentResponseBuilder responseBuilder)
    {
        responseBuilder.AppendMessage("Result of GetServiceInformation function:");
        const int anyMaster = 0;

        // GetCompanyId
        var companyId = parameter.PluginParameter.CompanyId;

        var foundServices = await GetServicesByQuery(
            parameter.ServiceQuery,
            companyId,
            anyMaster
        );

        if (foundServices.Count == 0)
        {
            responseBuilder.AppendMessage("Could not find services by query");
            return;
        }

        responseBuilder.AppendMessage("Found services: " + JsonSerializer.Serialize(
            foundServices.Select(x => new { x.Service, x.Category }),
            _jsonSerializerOptions));
    }

    public async Task GetAppointments(GetAppointmentsParameter parameter, AgentResponseBuilder responseBuilder)
    {
        responseBuilder.AppendMessage("Result of GetAppointments function:");

        // GetCompanyId
        var companyId = parameter.PluginParameter.CompanyId;

        var getAppointmentsComponent = await _components[nameof(GetAppointmentsComponent)]
            .InvokeAsync(new Hashtable
            {
                { GetAppointmentsComponent.AltegioCompanyIdParameterKey, companyId },
                { GetAppointmentsComponent.PhoneNumberParameterKey, parameter.PluginParameter.Dialogue.PhoneNumber }
            });

        responseBuilder.AppendMessage("Appointments: " + JsonSerializer.Serialize(
            getAppointmentsComponent.OrThrow().Result,
            _jsonSerializerOptions
        ));
    }

    public async Task StorePhoneNumber(StorePhoneNumberParameter parameter, AgentResponseBuilder responseBuilder)
    {
        if (string.IsNullOrWhiteSpace(parameter.PluginParameter.Dialogue.PhoneNumber))
        {
            parameter.PluginParameter.Dialogue.PhoneNumber =
                PhoneCorrector.ConvertPhoneNumberToDigits(parameter.PhoneNumber);

            _unitOfWork.GetRepository<Dialogue>().Update(parameter.PluginParameter.Dialogue);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task CancelAppointment(CancelAppointmentParameter parameter, AgentResponseBuilder responseBuilder)
    {
        responseBuilder.AppendMessage("Result of CancelAppointment function:");

        // GetCompanyId
        var companyId = parameter.PluginParameter.CompanyId;

        var appointmentIds = parameter.AppointmentIds.Split(',').Select(int.Parse).ToList();

        var cancelAppointmentsComponent = await _components[nameof(CancelAppointmentComponent)]
            .InvokeAsync(new Hashtable
            {
                { CancelAppointmentComponent.AltegioCompanyIdParameterKey, companyId },
                { CancelAppointmentComponent.RecordIdsParameterKey, appointmentIds }
            });

        responseBuilder.AppendMessage(
            "Result structure: Id - Appointment id, IsSuccess - Is appointment cancelled, Error message - Cause if appointment not cancelled, empty when success." +
            "Cancel appointments result: " +
            JsonSerializer.Serialize(cancelAppointmentsComponent.OrThrow().Result,
                _jsonSerializerOptions));
    }

    public async Task TransferAppointment(TransferAppointmentParameter parameter, AgentResponseBuilder responseBuilder)
    {
        responseBuilder.AppendMessage("Result of TransferAppointment function:");

        // GetCompanyId
        var companyId = parameter.PluginParameter.CompanyId;

        var appointmentId = parameter.AppointmentId;

        // GetDateByQuery
        var getDateByQueryComponentResult = await _components[nameof(GetDateByQueryComponent)].InvokeAsync(new Hashtable
        {
            { GetDateByQueryComponent.QueryParameterKey, $"{parameter.DateSlot} {parameter.TimeSlot}" },
            {
                GetDateByQueryComponent.TimezoneParameterKey,
                parameter.PluginParameter.Dialogue.CopilotChatBot.Timezone
            },
            { GetDateByQueryComponent.ConversationParameterKey, parameter.PluginParameter.Dialogue }
        });

        var date = (getDateByQueryComponentResult.OrThrow().Result as DateTimeOffset?)!;

        if (!date.HasValue)
        {
            responseBuilder.AppendMessage("Failed to understand date");
            return;
        }

        var transferAppointmentComponent = await _components[nameof(TransferAppointmentComponent)]
            .InvokeAsync(new Hashtable
            {
                { TransferAppointmentComponent.ExternalCompanyIdParameterKey, companyId },
                { TransferAppointmentComponent.RecordIdParameterKey, appointmentId },
                { TransferAppointmentComponent.DateParameterKey, date }
            });
        

        responseBuilder.AppendMessage("Transfer appointment result: " +
                                      JsonSerializer.Serialize(transferAppointmentComponent.OrThrow().Result,
                                          _jsonSerializerOptions));
    }

}