using OpenAI;
using OpenAI.Embeddings;
using RAGCosineSimilarity;

string apikey = "rdKgA";

var client = new OpenAIClient(apikey);

// Store all questions and their embeddings
List<QuestionEmbedding> embeddings = new();

Console.WriteLine("Generating embeddings...");
Console.WriteLine();

// Generate embedding for each stored question
foreach (var question in SampleQuestions.Questions)
{
    var response = await client
        .GetEmbeddingClient("text-embedding-3-small")
        .GenerateEmbeddingAsync(question);

    embeddings.Add(new QuestionEmbedding
    {
        Question = question,
        Vector = response.Value.ToFloats().ToArray()
    });

    Console.WriteLine($"Generated -> {question}");
}

Console.WriteLine();
Console.WriteLine("===========================================");
Console.WriteLine("Embeddings Generated Successfully");
Console.WriteLine("===========================================");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("-------------------------------------------");
    Console.Write("Ask a question (type 'exit' to quit): ");

    string? userQuestion = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userQuestion))
        continue;

    if (userQuestion.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    // Generate embedding for user query
    var queryResponse = await client
        .GetEmbeddingClient("text-embedding-3-small")
        .GenerateEmbeddingAsync(userQuestion);


    var queryVector = queryResponse.Value.ToFloats().ToArray();

    // Compare with every stored question
    var results = embeddings
        .Select(x => new
        {
            x.Question,
            Score = VectorMath.CosineSimilarity(queryVector, x.Vector)
        })
        .OrderByDescending(x => x.Score)
        .ToList();

    Console.WriteLine();
    Console.WriteLine("Top Matching Questions");
    Console.WriteLine("==============================================================");

    foreach (var item in results.Take(10))
    {
        string similarity = item.Score switch
        {
            >= 0.90 => "🟢 Very Similar",
            >= 0.75 => "🟡 Similar",
            >= 0.50 => "🟠 Somewhat Related",
            _ => "🔴 Unrelated"
        };

        Console.WriteLine($"{item.Score:F4}   {similarity,-20}   {item.Question}");
    }
}

Console.WriteLine("Finished.");