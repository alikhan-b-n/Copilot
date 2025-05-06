using System.Text.Json.Serialization;
using Copilot.Infrastructure.Entities;

namespace Copilot.AI.Plugins.Interfaces;

public interface ICopilotPlugin
{
    void SetParameters<T>(T parameters) where T : ParameterBase;
    
    public abstract class ParameterBase
    {
        [JsonIgnore]
        public string ClientMessage { get; set; }
        
        [JsonIgnore]
        public Dialogue Dialogue { get; set; }
    }
}