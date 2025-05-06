using System.ComponentModel;
using Copilot.AI.Plugins.Interfaces;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;
using Copilot.Experimental.Agents.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Experimental.Agents;

namespace Copilot.AI.Plugins.Plugins;

public class SalonPlugin : ICopilotPlugin
{
    private readonly ILogger<SalonPlugin> _logger;
    private readonly IAltegioPluginService _pluginService;

    private SalonPluginParameter _parameter;

    public SalonPlugin(ILogger<SalonPlugin> logger, IAltegioPluginService pluginService)
    {
        _logger = logger;
        _pluginService = pluginService;
    }

    public class SalonPluginParameter : ICopilotPlugin.ParameterBase
    {
        public int CompanyId { get; set; }
    }

    public void SetParameters<T>(T parameters) where T : ICopilotPlugin.ParameterBase
    {
        _parameter = (parameters as SalonPluginParameter)!;
    }

    [KernelFunction, Description("Find free slot for appointment. " +
                                 "Returns slots with masterID, serviceID, dateSlot and timeSlot.")]
    public async Task<AgentResponse> SearchSlot(Kernel kernel,
        [Description("masterQuery: The master's name or specialization, e.g. Ivan.")]
        string masterQuery = "",
        [Description("serviceQuery: The service, e.g. Haircut.")]
        string serviceQuery = "",
        [Description("whenQuery: Which date to check, e.g. tomorrow, 24.02. Default: now.")]
        string whenQuery = "now"
    ) => await Catch(nameof(SearchSlot), async responseBuilder =>
    {
        _logger.LogInformation(message:
            $"Function called. " +
            $"Params: masterQuery:'{masterQuery}', serviceQuery:'{serviceQuery}', whenQuery:'{whenQuery}'.");

        await _pluginService.GetSlots(new GetSlotsParameter
        {
            Kernel = kernel,
            PluginParameter = _parameter,
            MasterQuery = masterQuery,
            ServiceQuery = serviceQuery,
            WhenQuery = whenQuery
        }, responseBuilder);

        return responseBuilder.Build(_parameter.Dialogue.ThreadId);
    });


    [KernelFunction, Description("Appointment creation.")]
    public async Task<AgentResponse> CreateAppointment(Kernel kernel,
        [Description("masterID: Unique identifier of master. Can get from slot. 0 if any master.")]
        int masterId = 0,
        [Description("serviceID: Unique identifier of service. Can get from slot. 0 if service not found.")]
        int serviceId = 0,
        [Description("dateSlot: Date of slot. Must be in \'yyyyMMdd\' format. Can get from slot.")]
        string dateSlot = "",
        [Description("timeSlot: Time of slot. Must be in \'HH:mm\' format. Can get from slot.")]
        string timeSlot = "",
        [Description("client_name: Name of client. Empty if not sent.")]
        string name = "",
        [Description("client_phone: Phone number of client. Empty if not sent.")]
        string phone = ""
    ) => await Catch(nameof(CreateAppointment), async responseBuilder =>
    {
        _logger.LogInformation(message:
            $"Function called. " +
            $"Params: masterId:'{masterId}', serviceId:'{serviceId}', dateSlot:'{dateSlot}', " +
            $"timeSlot:'{timeSlot}', name:'{name}', phone:'{phone}'.");

        await _pluginService.CreateAppointment(new CreateAppointmentParameter
        {
            Kernel = kernel,
            PluginParameter = _parameter,
            MasterId = masterId,
            ServiceId = serviceId,
            DateSlot = dateSlot,
            TimeSlot = timeSlot,
            ClientName = name,
            ClientPhone = phone
        }, responseBuilder);

        var result = responseBuilder.Build(_parameter.Dialogue.ThreadId);

        _logger.LogInformation(message: "Result message: " + result.Message);
        _logger.LogInformation(message: "Result instruction: " + result.Instructions);

        return result;
    });

    [KernelFunction, Description(
         "Cancel appointment. Extremely undesirable function, should be called only when necessary. " +
         "You should try to keep the client by offering to reschedule the appointment.")]
    public async Task<AgentResponse> CancelAppointment(Kernel kernel,
        [Description(
            "appointmentsQuery: Which record or records client wants to cancel. " +
            "Can get from GetAppointments. Must be in comma separated format of record ids.")]
        string appointmentsQuery = ""
    ) => await Catch(nameof(CancelAppointment), async responseBuilder =>
    {
        _logger.LogInformation(message:
            $"Function called. " +
            $"Params: appointmentsQuery:'{appointmentsQuery}'.");

        await _pluginService.CancelAppointment(new CancelAppointmentParameter
        {
            Kernel = kernel,
            PluginParameter = _parameter,
            AppointmentIds = appointmentsQuery
        }, responseBuilder);

        return responseBuilder.Build(_parameter.Dialogue.ThreadId);
    });

    [KernelFunction, Description("Transfer appointment.")]
    public async Task<AgentResponse> TransferAppointment(Kernel kernel,
        [Description(
            "appointmentQuery: Which record client wants to transfer. Can get from GetAppointments. " +
            "Must be in comma separated format of record ids.")]
        int appointmentQuery = 0,
        [Description(
            "dateSlot: Date of slot to which we are transferring the record. Must be in \'yyyyMMdd\' format. " +
            "Can get from slot. Required.")]
        string dateSlot = "",
        [Description(
            "timeSlot: Time of slot to which we are transferring the record. Must be in \'HH:mm\' format. " +
            "Can get from slot. Required.")]
        string timeSlot = ""
    ) => await Catch(nameof(TransferAppointment), async responseBuilder =>
    {
        _logger.LogInformation(message:
            $"TransferAppointment. Function called. " +
            $"Params: appointmentQuery:'{appointmentQuery}', dateSlot:'{dateSlot}', timeSlot:'{timeSlot}'.");

        await _pluginService.TransferAppointment(new TransferAppointmentParameter
        {
            Kernel = kernel,
            PluginParameter = _parameter,
            AppointmentId = appointmentQuery,
            DateSlot = dateSlot,
            TimeSlot = timeSlot
        }, responseBuilder);

        return responseBuilder.Build(_parameter.Dialogue.ThreadId);
    });

    [KernelFunction, Description("Get client's appointments.")]
    public async Task<AgentResponse> GetAppointments(Kernel kernel) => await Catch(nameof(GetAppointments),
        async responseBuilder =>
        {
            _logger.LogInformation(message: "GetAppointments. Function called.");

            await _pluginService.GetAppointments(new GetAppointmentsParameter
            {
                Kernel = kernel,
                PluginParameter = _parameter
            }, responseBuilder);

            return responseBuilder.Build(_parameter.Dialogue.ThreadId);
        });

    [KernelFunction, Description("Get additional information about master(s).")]
    public async Task<AgentResponse> GetMasterInformation(Kernel kernel,
        [Description("masterQuery: The master's name or specialization, e.g. Ivan.")]
        string masterQuery = ""
    ) => await Catch(nameof(GetMasterInformation), async responseBuilder =>
    {
        _logger.LogInformation(message: $"Function called. Params: masterQuery:'{masterQuery}'.");

        await _pluginService.GetMasterInformation(new GetMasterInformationParameter
        {
            Kernel = kernel,
            PluginParameter = _parameter,
            MasterQuery = masterQuery
        }, responseBuilder);

        return responseBuilder.Build(_parameter.Dialogue.ThreadId);
    });

    [KernelFunction, Description("Get additional information about service(s).")]
    public async Task<AgentResponse> GetServiceInformation(Kernel kernel,
        [Description("serviceQuery: The service, e.g. Haircut. Empty if any.")]
        string serviceQuery = "") =>
        await Catch(nameof(GetServiceInformation), async responseBuilder =>
        {
            _logger.LogInformation(message:
                $"Function called. Params: serviceQuery:'{serviceQuery}'.");

            await _pluginService.GetServiceInformation(
                new GetServiceInformationParameter
                {
                    Kernel = kernel,
                    PluginParameter = _parameter,
                    ServiceQuery = serviceQuery
                }, responseBuilder);

            return responseBuilder.Build(_parameter.Dialogue.ThreadId);
        });

    [KernelFunction, Description("Store client's phone number when sent.")]
    public async Task<AgentResponse> StorePhoneNumber(
        [Description("phoneNumber: User's phone number, e.g. +70001234567")]
        string phoneNumber)
        => await Catch(nameof(StorePhoneNumber), async responseBuilder =>
        {
            _logger.LogInformation(message: $"Function called. Params: phoneNumber:'{phoneNumber}'.");

            await _pluginService.StorePhoneNumber(new StorePhoneNumberParameter
            {
                PhoneNumber = phoneNumber,
                PluginParameter = _parameter,
                Kernel = null!
            }, responseBuilder);

            return responseBuilder.Build(_parameter.Dialogue.ThreadId);
        });


    private async Task<AgentResponse> Catch(string functionName, Func<AgentResponseBuilder, Task<AgentResponse>> foo)
    {
        var responseBuilder = new AgentResponseBuilder();
        try
        {
            return await foo(responseBuilder);
        }
        catch (ExpectedException e)
        {
            _logger.LogInformation(functionName, e.Message);

            responseBuilder
                .AppendMessage(e.Message)
                .AppendInstructions(e.Instructions);

            return responseBuilder.Build(_parameter.Dialogue.ThreadId);
        }
        catch (Exception e)
        {
            _logger.LogError(functionName, e, "YClientsSalonPlugin");

            responseBuilder
                .AppendMessage("Error: Sorry, something went wrong :(")
                .AppendInstructions("Failed to run function");

            return responseBuilder.Build(_parameter.Dialogue.ThreadId);
        }
    }
}