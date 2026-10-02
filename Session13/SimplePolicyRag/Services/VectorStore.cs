using SimplePolicyRag.Models;
using OpenAI.Embeddings;

namespace SimplePolicyRag.Services;

/// <summary>
/// In-memory vector database service handling OpenAI embedding generation and Cosine Similarity vector retrieval.
/// </summary>
public class VectorStore
{
    private readonly EmbeddingClient _embeddingClient;
    private readonly List<VectorDB> _chunks = new();

    public VectorStore(EmbeddingClient embeddingClient)
    {
        _embeddingClient = embeddingClient;
    }

    /// <summary>
    /// Gets all indexed chunks stored in memory.
    /// </summary>
    public IReadOnlyList<VectorDB> Chunks => _chunks.AsReadOnly();

    /// <summary>
    /// Ingests document chunks into memory by generating vector embeddings for each chunk via OpenAI API.
    /// </summary>
    public async Task IngestChunksAsync(IEnumerable<VectorDB> chunks, CancellationToken cancellationToken = default)
    {
        foreach (var chunk in chunks)
        {
            OpenAIEmbedding embedding = await _embeddingClient.GenerateEmbeddingAsync(chunk.Text, cancellationToken: cancellationToken);
            chunk.Embedding = embedding.ToFloats().ToArray();
            _chunks.Add(chunk);
        }
    }

    /// <summary>
    /// Manually registers a pre-computed vector chunk (useful for unit testing).
    /// </summary>
    public void AddChunkWithEmbedding(VectorDB chunk)
    {
        _chunks.Add(chunk);
    }

    /// <summary>
    /// Searches for top-K chunks most semantically relevant to the user query using Cosine Similarity.
    /// </summary>
    public async Task<List<SearchResult>> SearchAsync(string queryText, int topK = 3, CancellationToken cancellationToken = default)
    {
        if (_chunks.Count == 0) return new List<SearchResult>();

        // 1. Embed query
        OpenAIEmbedding queryEmbedding = await _embeddingClient.GenerateEmbeddingAsync(queryText, cancellationToken: cancellationToken);
        float[] queryVector = queryEmbedding.ToFloats().ToArray();

        // 2. Rank all chunks by Cosine Similarity
        return SearchWithVector(queryVector, topK);
    }

    /// <summary>
    /// Ranks chunks using a raw query vector (used for search and unit testing).
    /// </summary>
    public List<SearchResult> SearchWithVector(float[] queryVector, int topK = 3)
    {
        var results = new List<SearchResult>();

        foreach (var chunk in _chunks)
        {
            float sim = ComputeCosineSimilarity(queryVector, chunk.Embedding);
            results.Add(new SearchResult(chunk, sim));
        }

        return results
            .OrderByDescending(r => r.SimilarityScore)
            .Take(topK)
            .ToList();
    }

    /// <summary>
    /// Calculates Cosine Similarity between two float vectors.
    /// CosineSimilarity(A, B) = (A • B) / (||A|| * ||B||)
    /// </summary>
    public static float ComputeCosineSimilarity(ReadOnlySpan<float> vectorA, ReadOnlySpan<float> vectorB)
    {
        if (vectorA.Length != vectorB.Length)
            throw new ArgumentException($"Vector dimensions must match. A: {vectorA.Length}, B: {vectorB.Length}");

        float dotProduct = 0.0f;
        float normA = 0.0f;
        float normB = 0.0f;

        for (int i = 0; i < vectorA.Length; i++)
        {
            dotProduct += vectorA[i] * vectorB[i];
            normA += vectorA[i] * vectorA[i];
            normB += vectorB[i] * vectorB[i];
        }

        if (normA == 0.0f || normB == 0.0f) return 0.0f;

        return dotProduct / ((float)Math.Sqrt(normA) * (float)Math.Sqrt(normB));
    }
}
