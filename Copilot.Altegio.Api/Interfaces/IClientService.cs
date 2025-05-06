using Copilot.Altegio.Api.Models;
using Copilot.Altegio.Api.Models.ClientService;

namespace Copilot.Altegio.Api.Interfaces;

public interface IClientService
{
    /// <summary>
    /// Retrieves clients based on the specified criteria.
    /// </summary>
    /// <param name="companyId">The ID of the company.</param>
    /// <param name="count">The number of clients to retrieve. Default is null.</param>
    /// <param name="page">The page number of the clients to retrieve. Default is null.</param>
    /// <param name="fullname">The full name of the clients to retrieve. Default is null.</param>
    /// <param name="phone">The phone number of the clients to retrieve. Default is null.</param>
    /// <param name="email">The email address of the clients to retrieve. Default is null.</param>
    /// <returns>An AltegioResponse containing a list of ClientResponse objects.</returns>
    Task<AltegioResponse<ClientResponse>> GetClientsAsync(
        int companyId,
        int? count = null,
        int? page = null,
        string fullname = null,
        string phone = null,
        string email = null);
}