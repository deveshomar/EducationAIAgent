using VectorDB_RAG;

string apiKey = "sk-proj-";

var embeddingService =
    new EmbeddingService(apiKey);

var qdrant =
    new QdrantService(
        "localhost",
        6334,
        "Policydocument");

await qdrant.CreateCollectionAsync();

var chunkService = new ChunkService();

var chunks =
    chunkService.ReadChunks("D:\\Sessions\\Education\\Session15\\RAGAPP_QUDARNT\\VectorDB_RAG\\VectorDB_RAG\\Policy.txt");

foreach (var chunk in chunks)
{
    Console.WriteLine(chunk.Text);

    chunk.Embedding =
        await embeddingService.GenerateEmbeddingAsync(
            chunk.Text);

    await qdrant.SaveAsync(
        chunk.Id,
        chunk.Embedding,
        chunk.Text);

    Console.WriteLine($"Saved {chunk.Id}");
}

Console.WriteLine("Vector generation done for these document:");


Console.WriteLine("Question:");

string question = Console.ReadLine();

var vector =
    await embeddingService.GenerateEmbeddingAsync(question);

var results =
    await qdrant.SearchAsync(vector);

foreach (var item in results)
{
    Console.WriteLine("----------------");

    Console.WriteLine(item.Score);

    Console.WriteLine(
        item.Payload["text"].StringValue);
}

