using System.Reflection;
using Copilot.AI.Plugins.Plugins;

namespace Copilot.AI.Plugins;

/// <summary>
/// Provides a collection of pre-defined plugin templates for Copilot.
/// </summary>
public static partial class PluginTemplates
{
    public static PluginTemplate SalonPlugin = new(
        Name: "salon",
        PluginType: typeof(SalonPlugin),
        ParameterDataType: typeof(SalonPlugin.SalonPluginParameter)
    );

    public static PluginTemplate OperatorPlugin = new(
        Name: "operator",
        PluginType: typeof(OperatorPlugin),
        ParameterDataType: typeof(OperatorPlugin.OperatorPluginParameter)
    );

    // A plugin name can contain only ASCII letters, digits, and underscores
    // Add other plugins here...
}

#region Model and System Methods

public record PluginTemplate(string Name, Type PluginType, Type ParameterDataType);

/// <summary>
/// Utility class containing methods to retrieve plugin templates.
/// </summary>
public static partial class PluginTemplates
{
    public static Dictionary<string, PluginTemplate> GetAllAsDictionary()
    {
        return typeof(PluginTemplates)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.FieldType == typeof(PluginTemplate))
            .Select(x => x.GetValue(null))
            .OfType<PluginTemplate>()
            .ToDictionary(x => x.Name, x => x);
    }

    public static List<PluginTemplate> GetAllAsList()
    {
        return typeof(PluginTemplates)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.FieldType == typeof(PluginTemplate))
            .Select(x => x.GetValue(null))
            .OfType<PluginTemplate>()
            .ToList();
    }
}

#endregion