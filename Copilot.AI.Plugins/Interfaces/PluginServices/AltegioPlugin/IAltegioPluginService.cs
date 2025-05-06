using Copilot.AI.Plugins.Plugins;
using Copilot.Experimental.Agents.Extensions;
using Microsoft.SemanticKernel;

namespace Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;

/// <summary>
/// Represents a service for creating records in the Altegio system.
/// </summary>
public interface IAltegioPluginService
{
    /// <summary>
    /// Retrieves the available slots based on the specified parameters.
    /// </summary>
    /// <param name="parameter">The parameter object containing the query information.</param>
    /// <param name="agentResponseBuilder"></param>
    /// <returns>A Task representing the asynchronous operation. The result contains the available slots as a string.</returns>
    Task GetSlots(GetSlotsParameter parameter, AgentResponseBuilder agentResponseBuilder);

    /// <summary>
    /// Creates an appointment in the Altegio system.
    /// </summary>
    /// <param name="parameter">The parameter object containing the appointment details.</param>
    /// <param name="agentResponseBuilder">The builder for creating AgentResponse instances.</param>
    /// <returns>A Task representing the asynchronous operation. The result contains the created appointment details.</returns>
    Task CreateAppointment(CreateAppointmentParameter parameter, AgentResponseBuilder agentResponseBuilder);

    /// <summary>
    /// Retrieves the master information based on the specified query.
    /// </summary>
    /// <param name="parameter">The parameter object containing the master query.</param>
    /// <param name="agentResponseBuilder">The builder for creating AgentResponse instances.</param>
    /// <returns>A Task representing the asynchronous operation. The result contains the master information.</returns>
    Task GetMasterInformation(GetMasterInformationParameter parameter, AgentResponseBuilder agentResponseBuilder);

    /// <summary>
    /// Retrieves the information about a specific service from the Altegio system.
    /// </summary>
    /// <param name="parameter">The parameter object containing the service query information.</param>
    /// <param name="responseBuilder">The builder for creating AgentResponse instances.</param>
    /// <returns>A Task representing the asynchronous operation. The result contains the service information as a string.</returns>
    Task GetServiceInformation(GetServiceInformationParameter parameter, AgentResponseBuilder responseBuilder);

    /// <summary>
    /// Retrieves the appointments based on the specified parameters.
    /// </summary>
    /// <param name="parameter">The parameter object containing the query information.</param>
    /// <param name="responseBuilder">The builder for creating AgentResponse instances.</param>
    /// <returns>A Task representing the asynchronous operation. The result contains the appointments as a string.</returns>
    Task GetAppointments(GetAppointmentsParameter parameter, AgentResponseBuilder responseBuilder);

    /// <summary>
    /// Stores the phone number in the Altegio system.
    /// </summary>
    /// <param name="parameter">The parameter object containing the phone number.</param>
    /// <param name="responseBuilder">The builder for creating AgentResponse instances.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameter is null.</exception>
    Task StorePhoneNumber(StorePhoneNumberParameter parameter, AgentResponseBuilder responseBuilder);

    /// <summary>
    /// Cancels an appointment in the Altegio system.
    /// </summary>
    /// <param name="parameter">The parameter object containing the appointment details.</param>
    /// <param name="responseBuilder">The builder for creating AgentResponse instances.</param>
    /// <returns>A Task representing the asynchronous operation. The result contains the canceled appointment details.</returns>
    Task CancelAppointment(CancelAppointmentParameter parameter, AgentResponseBuilder responseBuilder);

    /// <summary>
    /// Transfers an appointment in the Altegio system.
    /// </summary>
    /// <param name="parameter">The parameter object containing the appointment details.</param>
    /// <param name="responseBuilder">The builder for creating AgentResponse instances.</param>
    /// <returns>A Task representing the asynchronous operation. The result contains the transfered appointment details.</returns>
    Task TransferAppointment(TransferAppointmentParameter parameter, AgentResponseBuilder responseBuilder);
}

public abstract class CreateRecordParameter
{
    public Kernel Kernel { get; set; }
    public SalonPlugin.SalonPluginParameter PluginParameter { get; set; }
}

public class GetSlotsParameter : CreateRecordParameter
{
    public string MasterQuery { get; set; } = string.Empty;
    public string ServiceQuery { get; set; } = string.Empty;
    public string WhenQuery { get; set; } = string.Empty;
}

public class GetServiceInformationParameter : CreateRecordParameter
{
    public string ServiceQuery { get; set; } = string.Empty;
}

public class GetMasterInformationParameter : CreateRecordParameter
{
    public string MasterQuery { get; set; } = string.Empty;
}

public class GetAppointmentsParameter : CreateRecordParameter
{
}

public class StorePhoneNumberParameter : CreateRecordParameter
{
    public string PhoneNumber { get; set; } = string.Empty;
}

public class CreateAppointmentParameter : CreateRecordParameter
{
    public int MasterId { get; set; }
    public int ServiceId { get; set; }

    public string DateSlot { get; set; } = string.Empty;
    public string TimeSlot { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;
}

public class CancelAppointmentParameter : CreateRecordParameter
{
    public string AppointmentIds { get; set; }
}

public class TransferAppointmentParameter : CreateRecordParameter
{
    public int AppointmentId { get; set; }
    public string DateSlot { get; set; } = string.Empty;
    public string TimeSlot { get; set; } = string.Empty;
}
