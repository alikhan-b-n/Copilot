using Copilot.Contracts.Parameters;

namespace Copilot.Contracts.Responses;

public class AllPluginsModel
{
    public Guid? SalonPluginId { get; set; }
    public SalonPluginParameter? SalonPlugin { get; set; }
    
    public Guid? OperatorPluginId { get; set; }
    public OperatorPluginParameter? OperatorPlugin { get; set; }
}