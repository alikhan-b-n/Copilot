namespace Copilot.Contracts.Parameters;

public class SalonPluginParameter
{
    public int CompanyId { get; set; }
}

public class OperatorPluginParameter
{
    public IEnumerable<string> NumberList { get; set; } = new List<string>();

    public Guid WhatsAppAccountId { get; set; } = Guid.Empty;
}