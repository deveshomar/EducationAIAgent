using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using SimplePolicyRag.Services;
using SimplePolicyRag.Tests;

namespace SimplePolicyRag;

class Program
{
    static async Task Main(string[] args)
    {
        //Console.OutputEncoding = System.Text.Encoding.UTF8;

        //if (args.Length > 0 && args[0].Equals("--test", StringComparison.OrdinalIgnoreCase))
        //{
        //    bool testPassed = await RagVerificationTests.RunAllTestsAsync();
        //    Environment.Exit(testPassed ? 0 : 1);
        //    return;
        //}

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
================================================================================
          📄 SIMPLE C# RAG (Retrieval-Augmented Generation) SYSTEM
================================================================================
 Vector Embeddings (text-embedding-3-small) + Cosine Similarity + Chat (gpt-4o-mini)
--------------------------------------------------------------------------------");
        Console.ResetColor();

        // 1. Get OpenAI API Key
         string apiKey = "rdKgA";

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[Notice] OPENAI_API_KEY environment variable is not set.");
            Console.Write("👉 Please enter your OpenAI API Key: ");
            Console.ResetColor();

            apiKey = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: OpenAI API key is required. Exiting.");
                Console.ResetColor();
                return;
            }
        }

        // 2. Load Policy Document
        string policyPath = @"D:\Sessions\Education\Session13\SimplePolicyRag\Data\company_policy.md";



        if (!File.Exists(policyPath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: Policy document not found at '{policyPath}'.");
            Console.ResetColor();
            return;
        }

        string rawDocumentText = await File.ReadAllTextAsync(policyPath);

        // 3. Configure Services
        var openAIClient = new OpenAIClient(apiKey);
        var embeddingClient = openAIClient.GetEmbeddingClient("text-embedding-3-small");
        var chatClient = openAIClient.GetChatClient("gpt-4o-mini");

        var vectorStore = new VectorStore(embeddingClient);
        var ragEngine = new RagEngine(vectorStore, chatClient);

        // 4. Document Chunking & Vector Ingestion
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[Phase 1] Chunking Policy Document...");
        var chunks = TextChunkerParser.ChunkParseDocument(rawDocumentText);
        Console.WriteLine($"  ✓ Extracted {chunks.Count} document text chunks.");

        Console.WriteLine("\n[Phase 2] Generating OpenAI Vector Embeddings (text-embedding-3-small)...");
        await vectorStore.IngestChunksAsync(chunks);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  ✓ Ingested {vectorStore.Chunks.Count} chunks into In-Memory Vector Store.");
        Console.ResetColor();

    

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("User Question > ");
            Console.ResetColor();

            string? question = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(question)) continue;
            if (question.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                question.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting Simple Policy RAG. Goodbye!");
                break;
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n[Step 1: Embedding Query & Calculating Cosine Similarity Vectors...]");
                Console.ResetColor();

                var ragResult = await ragEngine.AskQuestionAsync(question);

                // Display Retrieved Chunks with Similarity Scores
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n🔍 RETRIEVED CONTEXT CHUNKS (Top Vector Matches):");
                foreach (var match in ragResult.RetrievedContexts)
                {
                    Console.WriteLine($" • [{match.Chunk.Id}] {match.Chunk.SectionTitle} (Similarity Score: {match.SimilarityScore:P2})");
                }
                Console.ResetColor();

                // Display Final AI Answer
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n🤖 RAG ANSWER:\n{ragResult.Answer}\n");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error processing RAG request: {ex.Message}\n");
                Console.ResetColor();
            }
        }
    }
}
