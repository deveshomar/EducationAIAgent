#pragma warning disable OPENAI001

using OpenAI.Embeddings;
using OpenAI.Responses;
using System.Numerics;

// ============================================================
// SIMPLE CRAG DEMO
// User Question
//      ↓
// Query Rewriting
//      ↓
// Embedding
//      ↓
// Cosine Similarity
//      ↓
// Retrieve relevant chunks
//      ↓
// Generate Answer
// ============================================================

string apiKey = "sk-proj-";

string chatModel = "gpt-5-mini";
string embeddingModel = "text-embedding-3-small";

// ------------------------------------------------------------
// OpenAI Clients
// ------------------------------------------------------------

var embeddingClient = new EmbeddingClient(
    model: embeddingModel,
    apiKey: apiKey);

var responsesClient = new ResponsesClient(apiKey);


// ------------------------------------------------------------
// 1. HR POLICY DOCUMENT
// ------------------------------------------------------------

var documents = new List<DocumentChunk>
{
    new DocumentChunk(
        "LEAVE-01",
        """
        SECTION LEAVE-01: ANNUAL LEAVE POLICY

        Full-time permanent employees receive twenty days of paid
        annual leave per calendar year.

        Annual leave is accrued monthly.

        Employees may carry forward a maximum of five unused annual
        leave days into the following calendar year.

        Unused carried-forward leave expires on March 31.

        Leave requests exceeding three consecutive business days
        must be submitted at least two weeks in advance through
        the HR Portal.

        Managers are responsible for approving or rejecting
        leave requests.
        """
    ),

    new DocumentChunk(
        "BENEFITS-01",
        """
        SECTION BENEFITS-01: HEALTH BENEFITS

        Full-time permanent employees are eligible for company
        health insurance.

        Health insurance includes medical and dental coverage.

        Employees can add eligible dependents to their health plan.
        """
    ),

    new DocumentChunk(
        "REMOTE-01",
        """
        SECTION REMOTE-01: REMOTE WORK POLICY

        Employees may work remotely up to three days per week,
        subject to manager approval.

        Remote work schedules must be agreed with the employee's
        manager.
        """
    ),

    new DocumentChunk(
        "HOLIDAY-01",
        """
        SECTION HOLIDAY-01: COMPANY HOLIDAYS

        The company observes public holidays according to the
        applicable national holiday calendar.

        Company holidays are separate from annual leave.
        """
    )
};


// ------------------------------------------------------------
// 2. CREATE EMBEDDINGS FOR DOCUMENT CHUNKS
// ------------------------------------------------------------

Console.WriteLine("Creating document embeddings...");

foreach (var document in documents)
{
    document.Embedding =
        await CreateEmbeddingAsync(
            embeddingClient,
            document.Text);
}

Console.WriteLine("Document embeddings created.");
Console.WriteLine();


// ------------------------------------------------------------
// 3. USER QUESTION
// ------------------------------------------------------------

string userQuestion = "Is healthcare included with my job?";

Console.WriteLine("==============================================");
Console.WriteLine("USER QUESTION");
Console.WriteLine("==============================================");
Console.WriteLine(userQuestion);
Console.WriteLine();


// ------------------------------------------------------------
// 4. CRAG QUERY REWRITING
// ------------------------------------------------------------

Console.WriteLine("==============================================");
Console.WriteLine("CRAG - REWRITING QUERY");
Console.WriteLine("==============================================");

string rewrittenQuery =
    await MakeLLMCall(
        responsesClient,
        chatModel,
        userQuestion);

Console.WriteLine(rewrittenQuery);
Console.WriteLine();


// ------------------------------------------------------------
// 5. EMBEDDING OF REWRITTEN QUERY
// ------------------------------------------------------------

Console.WriteLine("==============================================");
Console.WriteLine("CREATING QUERY EMBEDDING");
Console.WriteLine("==============================================");

float[] queryEmbedding =
    await CreateEmbeddingAsync(
        embeddingClient,
        rewrittenQuery);

Console.WriteLine(
    $"Embedding dimensions: {queryEmbedding.Length}");

Console.WriteLine();


// ------------------------------------------------------------
// 6. COSINE SIMILARITY SEARCH
// ------------------------------------------------------------

Console.WriteLine("==============================================");
Console.WriteLine("COSINE SIMILARITY SEARCH");
Console.WriteLine("==============================================");

var results = Search(
    documents,
    queryEmbedding,
    topK: 3);

foreach (var result in results)
{
    Console.WriteLine(
        $"{result.Document.Id} -> Score: {result.Score:F4}");
}

Console.WriteLine();


// ------------------------------------------------------------
// 7. SHOW RETRIEVED CONTEXT
// ------------------------------------------------------------

Console.WriteLine("==============================================");
Console.WriteLine("RETRIEVED CONTEXT");
Console.WriteLine("==============================================");

foreach (var result in results)
{
    Console.WriteLine();
    Console.WriteLine($"[{result.Document.Id}]");
    Console.WriteLine(result.Document.Text);
}

Console.WriteLine();


// ------------------------------------------------------------
// 8. GENERATE FINAL ANSWER
// ------------------------------------------------------------

Console.WriteLine("==============================================");
Console.WriteLine("FINAL ANSWER");
Console.WriteLine("==============================================");

string context = string.Join(
    "\n\n",
    results.Select(r =>
        $"[{r.Document.Id}]\n{r.Document.Text}"));

string answer =
    await GenerateAnswerAsync(
        responsesClient,
        chatModel,
        userQuestion,
        context);

Console.WriteLine(answer);


// ============================================================
// METHODS
// ============================================================


// ------------------------------------------------------------
// CREATE EMBEDDING
// ------------------------------------------------------------

static async Task<float[]> CreateEmbeddingAsync(
    EmbeddingClient client,
    string text)
{
    var embedding =
        await client.GenerateEmbeddingAsync(text);

    return embedding.Value.ToFloats().ToArray();
}


// ------------------------------------------------------------
// CRAG QUERY REWRITING
// ------------------------------------------------------------

static async Task<string> MakeLLMCall(
    ResponsesClient client,
    string model,
    string userQuestion)
{
    string prompt = $"""
        You are a query rewriting component in a RAG system.

        Your job is NOT to answer the question.

        Rewrite the user's question into a clear and specific
        search query that will work well with semantic vector
        search against an HR policy document.
        

        Preserve the user's original intent.

        Add relevant context when it can reasonably be inferred.

        Rule:  No more explanation, no commentary, no preamble, no postamble, no apologies.
               just re frame quesiton so that i will look like that user is asking for information about HR policy.
               you need to re write in such a way if quesiton appears in comeplete then re write it in such a way that it will be complete and clear.

        Return ONLY the rewritten search query.

        
        User question:
        {userQuestion}
        """;

    ResponseResult response =
        await client.CreateResponseAsync(
            model,
            prompt);

    return response.GetOutputText().Trim();
}


// ------------------------------------------------------------
// COSINE SIMILARITY SEARCH
// ------------------------------------------------------------

static List<SearchResult> Search(
    List<DocumentChunk> documents,
    float[] queryEmbedding,
    int topK)
{
    var results = documents
        .Select(document => new SearchResult
        {
            Document = document,
            Score = CosineSimilarity(
                queryEmbedding,
                document.Embedding!)
        })
        .OrderByDescending(x => x.Score)
        .Take(topK)
        .ToList();

    return results;
}


// ------------------------------------------------------------
// COSINE SIMILARITY
// ------------------------------------------------------------

static float CosineSimilarity(
    float[] vectorA,
    float[] vectorB)
{
    if (vectorA.Length != vectorB.Length)
        throw new ArgumentException(
            "Vectors must have the same dimensions.");

    float dotProduct = 0;
    float magnitudeA = 0;
    float magnitudeB = 0;

    for (int i = 0; i < vectorA.Length; i++)
    {
        dotProduct += vectorA[i] * vectorB[i];

        magnitudeA += vectorA[i] * vectorA[i];

        magnitudeB += vectorB[i] * vectorB[i];
    }

    if (magnitudeA == 0 || magnitudeB == 0)
        return 0;

    return dotProduct /
           (MathF.Sqrt(magnitudeA) *
            MathF.Sqrt(magnitudeB));
}


// ------------------------------------------------------------
// GENERATE FINAL ANSWER
// ------------------------------------------------------------

static async Task<string> GenerateAnswerAsync(
    ResponsesClient client,
    string model,
    string question,
    string context)
{
    string prompt = $"""
        You are an HR policy assistant.

        Answer the user's question using ONLY the provided
        policy context.

        If the answer is not available in the context,
        say that the policy does not specify the answer.

        Do not invent information.

        User Question:
        {question}

        Policy Context:
        {context}
        """;

    ResponseResult response =
        await client.CreateResponseAsync(
            model,
            prompt);

    return response.GetOutputText().Trim();
}


// ============================================================
// DATA CLASSES
// ============================================================

class DocumentChunk
{
    public string Id { get; }

    public string Text { get; }

    public float[]? Embedding { get; set; }

    public DocumentChunk(
        string id,
        string text)
    {
        Id = id;
        Text = text;
    }
}


class SearchResult
{
    public DocumentChunk Document { get; set; } = null!;

    public float Score { get; set; }
}