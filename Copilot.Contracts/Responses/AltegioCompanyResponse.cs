namespace Copilot.Contracts.Responses;

public class AltegioCompanyResponse
{
    public int CompanyId { get; set; }
    
    public string CompanyName { get; set; }

    public Guid UserId { get; set; }

    public Guid Id { get; set; }
}