using OpenAI.Chat;
using RAG_GoldenDataset.Model;
using RAG_GoldenDataset.Services;

using System.Text.Json;

string apiKey = "sk-pr";
var answerService = new RagAnswerService(apiKey);
var evaluator = new RagAnswerEvaluator(apiKey);
Console.WriteLine("=================================");
Console.WriteLine("      RAG EVALUATION DEMO");
Console.WriteLine("=================================");

// ---------------------------------------
// 1. INGEST DOCUMENT
// ---------------------------------------

var ingestion =
    new DocumentIngestionService();

string Policyfilepath = @"D:\\Sessions\\Education\\Session14\\RAG_GoldendataSet\\RAG_GoldenDataset\\RAG_GoldenDataset\\Data\\Policy.txt";

var chunks =
    ingestion.LoadAndChunk(Policyfilepath);

Console.WriteLine(
    $"Chunks created: {chunks.Count}");

foreach (var chunk in chunks)
{
    Console.WriteLine(
        $"{chunk.Id} -> {chunk.Section}");
}

// ---------------------------------------
// 2. CREATE EMBEDDINGS
// ---------------------------------------

var embeddingService =
    new EmbeddingService(apiKey);

var documentEmbeddingService =
    new DocumentEmbeddingService(
        embeddingService);

await documentEmbeddingService
    .CreateEmbeddingsAsync(chunks);

Console.WriteLine(
    "All embeddings created.");

// ---------------------------------------
// 3. CREATE VECTOR SEARCH
// ---------------------------------------

var searchService =
    new VectorSearchService(
        embeddingService);

// ---------------------------------------
// 4. LOAD GOLDEN DATASET
// ---------------------------------------
string goldenDatasetPath = @"D:\\Sessions\\Education\\Session14\\RAG_GoldendataSet\\RAG_GoldenDataset\\RAG_GoldenDataset\\Data\\Seperation.json";        

string json =
    await File.ReadAllTextAsync(
        goldenDatasetPath);

var goldenDataset =
     JsonSerializer.Deserialize<List<GoldenQuestion>>(
        json,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

if (goldenDataset == null)
{
    throw new Exception("Golden dataset could not be loaded.");
}
Console.WriteLine($"Loaded {goldenDataset.Count} golden questions.");
// ---------------------------------------
// 5. RUN EVALUATION
// ---------------------------------------

double totalPrecision = 0;
double totalRecall = 0;
double totalHitRate = 0;
double totalMRR = 0;

int k = 3;

foreach (var testCase in goldenDataset)
{
    Console.WriteLine();
    Console.WriteLine(
        $"Question {testCase.Id}:");

    Console.WriteLine(
        testCase.Question);

    var results =
        await searchService.SearchAsync(
            testCase.Question,
            chunks,
            k);

    var retrievedIds =
        results
            .Select(x => x.Chunk.Id)
            .ToList();

    Console.WriteLine(
        "Retrieved:");

    foreach (var result in results)
    {
        Console.WriteLine(
            $"  {result.Chunk.Id} " +
            $"Score={result.Score:F4}");
    }

    var metrics =
        RagEvaluationService.Calculate(
            retrievedIds,
            testCase.RelevantChunkIds,
            k);

    Console.WriteLine(
        $"Precision@{k}: " +
        $"{metrics.PrecisionAtK:F2}");

    Console.WriteLine(
        $"Recall@{k}: " +
        $"{metrics.RecallAtK:F2}");

    Console.WriteLine(
        $"Hit@{k}: " +
        $"{metrics.HitRateAtK:F2}");

    Console.WriteLine(
        $"RR: " +
        $"{metrics.ReciprocalRank:F2}");


    #region [Answers LLM validatiosn]


    // 2. Build context
    string context = string.Join(
        "\n\n",
        results.Select(r =>
            $"[{r.Chunk.Id}]\n{r.Chunk.Text}"));

    // 3. Generate answer
    string answer =
        await answerService.GenerateAnswerAsync(
            testCase.Question,
            context);

    // 4. Evaluate answer
    var evaluation =
        await evaluator.EvaluateAsync(
            testCase.Question,
            context,
            testCase.ExpectedAnswer,
            answer);

    Console.WriteLine("\n================================");
    Console.WriteLine($"Question: {testCase.Question}");
    Console.WriteLine($"Answer: {answer}");

    Console.WriteLine(
        $"Faithfulness : {evaluation.FaithfulnessScore}");

    Console.WriteLine(
        $"Relevance    : {evaluation.RelevanceScore}");

    Console.WriteLine(
        $"Correctness  : {evaluation.CorrectnessScore}");


    #endregion


    totalPrecision +=
        metrics.PrecisionAtK;

    totalRecall +=
        metrics.RecallAtK;

    totalHitRate +=
        metrics.HitRateAtK;

    totalMRR +=
        metrics.ReciprocalRank;
}

// ---------------------------------------
// 6. FINAL SCORE
// ---------------------------------------

int count = goldenDataset.Count;

Console.WriteLine();
Console.WriteLine("=================================");
Console.WriteLine("       FINAL RAG SCORECARD");
Console.WriteLine("=================================");

Console.WriteLine(
    $"Questions      : {count}");

Console.WriteLine(
    $"Precision@{k}   : " +
    $"{totalPrecision / count:P2}");

Console.WriteLine(
    $"Recall@{k}      : " +
    $"{totalRecall / count:P2}");

Console.WriteLine(
    $"Hit Rate@{k}    : " +
    $"{totalHitRate / count:P2}");

Console.WriteLine(
    $"MRR             : " +
    $"{totalMRR / count:P2}");