using OpenAI.Chat;
using OpenAI.Embeddings;
using RAGApp.Chunk;
using RAGApp.helper;
using RAGApp.RagCompare;


Console.WriteLine("Reading Company Policy...");
string apiKey = "sQAA";

// Read file
string filePath = "D:\\Sessions\\Proj\\session12\\RAGProject\\RAGApp\\RAGApp\\document\\policy.txt";

if (!File.Exists(filePath))
{
    Console.WriteLine("CompanyPolicy.txt not found.");
    return;
}

string document = File.ReadAllText(filePath);

// Split into chunks
List<string> chunks = Helper.SplitText(document, 150);

// Store chunks into memory
List<DocumentChunk> documentChunks = new();

int id = 1;

foreach (string chunk in chunks)
{
    documentChunks.Add(new DocumentChunk
    {
        Id = id++,
        Text = chunk
    });
}

// Display chunks
Console.WriteLine();
Console.WriteLine($"Total Chunks : {documentChunks.Count}");
Console.WriteLine();

foreach (DocumentChunk chunk in documentChunks)
{
    Console.WriteLine($"========== Chunk {chunk.Id} ==========");
    Console.WriteLine(chunk.Text);
    Console.WriteLine();
}



var embeddingClient = new EmbeddingClient(
    model: "text-embedding-3-small",
    apiKey: apiKey);

Console.WriteLine("Generating embeddings...\n");

foreach (var chunk in documentChunks)
{
    Console.WriteLine($"Processing Chunk {chunk.Id}...");

    var embedding = await embeddingClient.GenerateEmbeddingAsync(chunk.Text);

    chunk.Embedding = embedding.Value
                              .ToFloats()
                              .ToArray();

    Console.WriteLine($"Embedding Size : {chunk.Embedding.Length}");
}

Console.WriteLine();
Console.WriteLine("Embeddings Stored Successfully");
Console.WriteLine();

foreach (var chunk in documentChunks)
{
    Console.WriteLine($"Chunk Id : {chunk.Id}");
    Console.WriteLine($"Text : {chunk.Text}");

    Console.WriteLine($"Embedding Length : {chunk.Embedding?.Length}");

   

    for (int i = 0; i < documentChunks.Count; i++)
    {
        Console.Write($"{chunk.Embedding![i]:F4} ");
    }

    Console.WriteLine();
    Console.WriteLine(new string('-', 60));
}

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("RAG Search Ready");
Console.WriteLine("Type 'exit' to quit.");
Console.WriteLine("========================================");



while (true)
{
    Console.Write("\nAsk Question : ");

    string? question = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(question))
        continue;

    if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    //---------------------------------------------------------
    // Create embedding for question
    //---------------------------------------------------------

    var questionEmbeddingResponse =
        await embeddingClient.GenerateEmbeddingAsync(question);

    float[] questionEmbedding =
        questionEmbeddingResponse.Value.ToFloats().ToArray();

    //---------------------------------------------------------
    // Find similar chunks
    //---------------------------------------------------------

    var results = documentChunks
        .Select(chunk => new
        {
            Chunk = chunk,
            Score = CosineClass.CosineSimilarity(
                        questionEmbedding,
                        chunk.Embedding!)
        })
        .OrderByDescending(x => x.Score)
        .Take(3)
        .ToList();

    Console.WriteLine();
    Console.WriteLine("Top Matching Chunks");
    Console.WriteLine("-------------------------------------------");

    foreach (var item in results)
    {
        Console.WriteLine($"Score : {item.Score:F4}");
        Console.WriteLine(item.Chunk.Text);
        Console.WriteLine("-------------------------------------------");
    }

    //LLM Calls

    string context = string.Join(
    Environment.NewLine + Environment.NewLine,
    results.Select(x => x.Chunk.Text));

    string prompt = $"""
You are a helpful HR assistant.

Answer the user's question using ONLY the context below.

Context:
{context}

Question:
{question}
""";

    var chatClient = new ChatClient(
    model: "gpt-4.1-mini",
    apiKey: apiKey);

    var response = await chatClient.CompleteChatAsync(prompt);

    Console.WriteLine();
    Console.WriteLine("Answer:");
    Console.WriteLine(response.Value.Content[0].Text);
}

