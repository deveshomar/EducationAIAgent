using OpenAI.Embeddings;
string apikey = "A";

var client = new EmbeddingClient(
    model: "text-embedding-3-small",
    apiKey: apikey);

// Generate embeddings
var emb1 = await client.GenerateEmbeddingAsync(
    "i am feeling happy in this morning");

var emb2 = await client.GenerateEmbeddingAsync(
    "can i go for shopping");

// Extract vectors
ReadOnlyMemory<float> vector1 = emb1.Value.ToFloats();
ReadOnlyMemory<float> vector2 = emb2.Value.ToFloats();

// Calculate similarity
double similarity = CosineSimilarity(vector1.Span, vector2.Span);

Console.WriteLine($"Similarity = {similarity}");


static double CosineSimilarity(ReadOnlySpan<float> a, ReadOnlySpan<float> b)
{
    double dot = 0;
    double magA = 0;
    double magB = 0;

    for (int i = 0; i < a.Length; i++)
    {
        dot += a[i] * b[i];
        magA += a[i] * a[i];
        magB += b[i] * b[i];
    }

    return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
}