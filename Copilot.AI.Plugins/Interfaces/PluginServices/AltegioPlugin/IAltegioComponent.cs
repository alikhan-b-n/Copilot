using System.Collections;

namespace Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;

public interface IAltegioComponent
{
    public Type ResultType { get; }

    public Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable);
}

public static class HashtableExtensions
{
    public static T GetValueOrThrow<T>(this Hashtable hashtable, string key)
    {
        if (!hashtable.ContainsKey(key))
        {
            throw new ArgumentException($"No {key} parameter");
        }
        
        return (hashtable[key] is T? ? (T?)hashtable[key] : default) ?? throw new ArgumentException($"No {key} parameter");
    } 
    
    public static T? GetValueOrDefault<T>(this Hashtable hashtable, string key)
    {
        if (!hashtable.ContainsKey(key))
        {
            return default;
        }
        
        return (hashtable[key] is T ? (T)hashtable[key]! : default) ?? default;
    } 
}



public class AltegioComponentResult
{
    public bool IsSuccess { get; set; }
    public object? Result { get; set; }
    public string? ErrorMessage { get; set; }

    public AltegioComponentResult OrThrow()
    {
        if (!IsSuccess)
        {
            throw new ExpectedException(ErrorMessage ?? "Some errors occured");
        }

        return this;
    }
}