namespace Copilot.Altegio.Api.Interfaces;

public interface IAltegioAPI
{
    IRecordService RecordService { get; }
    IClientService ClientService { get; }
    IServicesService ServicesService { get; }
    IEmployeeService EmployeeService { get; }
    IBookformService BookformService { get; }
    IPartnerService PartnerService { get; }
    void SetDefaultParameters(string partnerToken, string userToken);
}