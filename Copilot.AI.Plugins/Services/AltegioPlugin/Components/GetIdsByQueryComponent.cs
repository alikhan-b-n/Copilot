using System.Collections;
using Copilot.AI.Plugins.Interfaces.PluginServices.AltegioPlugin;

namespace Copilot.AI.Plugins.Services.AltegioPlugin.Components;

public class GetIdsByQueryComponent : IAltegioComponent
{
    public Type ResultType { get; } = typeof(List<(int Id, string Info)>);

    public static readonly string DataSetKey = "dataSet";
    public static readonly string QueryKey = "query";
    public static readonly string MinSimilarityKey = "minSimilarity";

    /// <summary>
    /// Invokes the GetIdsByQueryCompany component asynchronously.
    /// </summary>
    /// <param name="hashtable">The hashtable containing the necessary parameters.</param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains the AltegioComponentResult.</returns>
    /// <exception cref="ArgumentException">Thrown when the dataSet or query parameters are not provided.</exception>
    public async Task<AltegioComponentResult> InvokeAsync(Hashtable hashtable)
    {
        await Task.CompletedTask;

        var dataSet = hashtable[DataSetKey] as List<(int Id, string Info)> ??
                      throw new ArgumentException("No dataSet parameter");
        var query = hashtable[QueryKey] as string ?? throw new ArgumentException("No query parameter");
        var minSimilarity = hashtable[MinSimilarityKey] as double? ?? SyntaxSearcher.MinSimilarityDefault;

        if (string.IsNullOrWhiteSpace(query))
            return new AltegioComponentResult
            {
                IsSuccess = true,
                Result = dataSet.Select(x => (x.Id, x.Info)).ToList(),
            };

        try
        {
            var result = SyntaxSearcher.Cosine(query, dataSet, minSimilarity: minSimilarity);

            return new AltegioComponentResult { IsSuccess = true, Result = result };
        }
        catch (Exception)
        {
            return new AltegioComponentResult
            {
                IsSuccess = false,
                ErrorMessage = "Failed to get needed data."
            };
        }
    }
}