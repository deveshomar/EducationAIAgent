namespace SimplePolicyRag.Models;

/// <summary>
/// Container for a vector similarity search result containing the matched chunk and score.
/// </summary>
public record SearchResult(
    VectorDB Chunk,
    float SimilarityScore
);
