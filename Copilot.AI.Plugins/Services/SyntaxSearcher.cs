using F23.StringSimilarity;

namespace Copilot.AI.Plugins.Services;

public static class SyntaxSearcher
{
    public const double MinSimilarityDefault = .28;
    public const double DeltaDefault = .03;
    
    /// <summary>
    /// Calculate the cosine similarity between a given query and a dataset.
    /// </summary>
    /// <param name="query">The query string to compare against the dataset.</param>
    /// <param name="dataSet">The dataset of (Id, Info) tuples to compare the query against.</param>
    /// <param name="minSimilarity">The minimum similarity score required for a match. Default is 0.4.</param>
    /// <param name="delta">The difference allowed from the top similarity score in the dataset. Default is 0.03.</param>
    /// <returns>A list of Ids that match the given query based on their cosine similarity scores.</returns>
    public static List<(int Id, string Info)> Cosine(
        string query, IEnumerable<(int Id, string Info)> dataSet,
        double minSimilarity = MinSimilarityDefault, double delta = DeltaDefault)
    {
        var cosine = new Cosine(2);
        var queryProfile = cosine.GetProfile(query);

        var found = dataSet
            // Transform each item into a tuple that contains its ID and its similarity to the query
            .Select(x => (x.Id, x.Info, Similarity: cosine.Similarity(cosine.GetProfile(x.Info), queryProfile)))
            .Where(x => x.Similarity > minSimilarity)
            .OrderBy(x => x.Similarity)
            .ToList();

        if (found.Count == 0) return new List<(int Id, string Info)>();

        // Defining a range for similarity values we are interested in
        var upperBound = found.MaxBy(x => x.Similarity).Similarity;
        var lowerBound = upperBound - delta;
        
        // Filter out items that fall below the similarity range  
        return found
            .Where(x => x.Similarity > lowerBound)
            .Select(x => (x.Id, x.Info))
            .ToList();
    }
}