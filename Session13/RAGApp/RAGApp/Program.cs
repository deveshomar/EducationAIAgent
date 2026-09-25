using OpenAI.Chat;
using OpenAI.Embeddings;
using RAGApp.Chunk;
using RAGApp.embeddings;
using RAGApp.helper;
using RAGApp.RagCompare;
using RAGApp_testCases.Evaluation;
using System.Text.Json;


Console.WriteLine("Reading Company Policy...");

string apiKey = "prdQAA";

// Read file
string filePath = "D:\\Sessions\\Proj\\session13\\RAGApp\\RAGApp\\document\\SamplePolicy.json";

if (!File.Exists(filePath))
{
    Console.WriteLine("file  not found.");
    return;
}


var json = File.ReadAllText(filePath);

var documentChunks = JsonSerializer.Deserialize<List<DocumentChunk>>(json)!;

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

}

Console.WriteLine();
Console.WriteLine("Embeddings Stored Successfully");
Console.WriteLine();

foreach (var chunk in documentChunks)
{
    Console.WriteLine($"Chunk Id : {chunk.Id}");
    Console.WriteLine($"Text : {chunk.Text}");

    Console.WriteLine($"Embedding Length : {chunk.Embedding?.Length}");

   

}


    foreach (var testCase in SampleRagDataSet.GetTestData())
    {
        Console.WriteLine();
        Console.WriteLine("=================================================");
        Console.WriteLine($"Question : {testCase.Question}");
        Console.WriteLine("=================================================");

        // Use the question from the evaluation dataset
        string questiondata = testCase.Question;

        //--------------------------------------------------------
        // Generate Question Embedding
        //--------------------------------------------------------

        var questionEmbeddingdata = await embeddingClient.GenerateEmbeddingAsync(questiondata);

        //--------------------------------------------------------
        // Retrieve Top K Chunks
        //--------------------------------------------------------

        var resultsdata= documentChunks
            .Select(chunk => new
            {
                Chunk = chunk,
                Score = CosineClass.CosineSimilarity(
                            questionEmbeddingdata.Value.ToFloats().ToArray(),
                            chunk.Embedding!)
            })
            .OrderByDescending(x => x.Score)
            .Take(3)
            .ToList();

        //--------------------------------------------------------
        // Display Retrieved Chunks
        //--------------------------------------------------------

        Console.WriteLine();
        Console.WriteLine("Retrieved Chunks");

        foreach (var item in resultsdata)
        {
            Console.WriteLine($"Chunk Id : {item.Chunk.Id}");
            Console.WriteLine($"Score    : {item.Score:F4}");
            Console.WriteLine(item.Chunk.Text);
            Console.WriteLine("------------------------------------");
        }

        //--------------------------------------------------------
        // Evaluate Retrieval
        //--------------------------------------------------------

        List<int> retrievedChunkIdsdata = resultsdata
            .Select(x => x.Chunk.Id)
            .ToList();

        double hitRate = RetrievalMetrics.HitRateAtK(
            retrievedChunkIdsdata,
            testCase.RelevantChunkIds);

        double precision = RetrievalMetrics.PrecisionAtK(
            retrievedChunkIdsdata,
            testCase.RelevantChunkIds);

        double recall = RetrievalMetrics.RecallAtK(
            retrievedChunkIdsdata,
            testCase.RelevantChunkIds);

        double mrr = RetrievalMetrics.ReciprocalRank(
            retrievedChunkIdsdata,
            testCase.RelevantChunkIds);

        double ndcg = RetrievalMetrics.NdcgAtK(
            retrievedChunkIdsdata,
            testCase.RelevantChunkIds);

        Console.WriteLine();
        Console.WriteLine("Retrieval Metrics");
        Console.WriteLine("---------------------------");
        Console.WriteLine($"HitRate   : {hitRate:F2}");
        Console.WriteLine($"Precision : {precision:F2}");
        Console.WriteLine($"Recall    : {recall:F2}");
        Console.WriteLine($"MRR       : {mrr:F2}");
        Console.WriteLine($"nDCG      : {ndcg:F2}");

        //--------------------------------------------------------
        // Build Context
        //--------------------------------------------------------

        string context = string.Join(
            Environment.NewLine + Environment.NewLine,
            resultsdata.Select(x => x.Chunk.Text));


    // CRAG
    string promptCRAG = $"""
You are an expert evaluator for a Retrieval-Augmented Generation (RAG) system.

Your task is to determine whether the retrieved context contains enough relevant information to answer the user's question.

Evaluation Criteria:

1. Relevance
- Does the context relate to the user's question?
- Score between 0 and 1.

2. Completeness
- Does the context contain sufficient information to answer the question completely?
- Score between 0 and 1.

3. Confidence
- Overall confidence that the context is sufficient for generating an accurate answer.
- Score between 0 and 1.

Instructions:
- Do NOT answer the user's question.
- Only evaluate the retrieved context.
- If the answer is partially available, mention what information is missing.
- If the context is unrelated, give low scores.
- Return ONLY valid JSON.
- Do not include markdown or explanations outside the JSON.

Return JSON in the following format:

    "isRelevant": true,
    "relevanceScore": 0.95,
    "completenessScore": 0.90,
    "confidenceScore": 0.93,
    "reason": "The retrieved context directly discusses the company's paid leave policy and contains enough information to answer the question.",
    "missingInformation": "",
    "recommendation": "ProceedToGeneration"


Possible recommendation values:
- ProceedToGeneration
- RetrieveMoreDocuments
- RewriteQuery
- PerformHybridSearch
- AskUserForClarification

Retrieved Context:
{context}

User Question:
{questiondata}
""";



    string prompt = $"""
    You are a helpful HR assistant.

    Answer the user's question using ONLY the context below.

    Context:
    {context}

    Question:
    {questiondata}
    """;

        //--------------------------------------------------------
        // Ask LLM
        //--------------------------------------------------------
        var chatClient = new ChatClient(
        model: "gpt-4.1-mini",
        apiKey: apiKey);
        var response = await chatClient.CompleteChatAsync(promptCRAG);

        Console.WriteLine();
        Console.WriteLine("LLM Answer");
        Console.WriteLine("---------------------------");
        Console.WriteLine(response.Value.Content[0].Text);
    }


    /*

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
    */


