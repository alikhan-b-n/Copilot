using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.EmployeeService;

namespace Copilot.Altegio.Api.Interfaces;

/// <summary>
/// Represents a service for managing employees.
/// </summary>
public interface IEmployeeService
{
    Task<AltegioResponse<EmployeeResponse>> GetEmployeesAsync(int companyId);
}