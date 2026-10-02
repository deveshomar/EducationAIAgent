using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplePolicyRag.Models;
using SimplePolicyRag.Services;

namespace SimplePolicyRag.Tests;

public static class RagVerificationTests
{
    public static Task<bool> RunAllTestsAsync()
    {
        Console.WriteLine("==================================================");
        Console.WriteLine("🧪 RUNNING RAG SYSTEM VERIFICATION TEST SUITE");
        Console.WriteLine("==================================================");

        int passed = 0;
        int total = 0;

        void Assert(bool condition, string testName)
        {
            total++;
            if (condition)
            {
                passed++;
                Console.WriteLine($"  [PASS] {testName}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  [FAIL] {testName}");
                Console.ResetColor();
            }
        }

        // Test 1: Document Chunking
        string sampleMd = "# Header 1\nSection 1 text content.\n\n## Section 2\nSection 2 text content.";
        var chunks = TextChunkerParser.ChunkParseDocument(sampleMd);
        Assert(chunks.Count == 2, "TextChunker extracts correct chunk count");
        Assert(chunks[0].Text == "Section 1 text content.", "TextChunker chunk 1 content match");
        Assert(chunks[1].SectionTitle == "Section 2", "TextChunker chunk 2 section title match");

        // Test 2: Cosine Similarity Vector Math
        float[] vectorA = new float[] { 1.0f, 0.0f, 0.0f };
        float[] vectorIdentical = new float[] { 1.0f, 0.0f, 0.0f };
        float[] vectorOrthogonal = new float[] { 0.0f, 1.0f, 0.0f };
        float[] vectorSimilar = new float[] { 0.9f, 0.1f, 0.0f };

        float simIdentical = VectorStore.ComputeCosineSimilarity(vectorA, vectorIdentical);
        Assert(Math.Abs(simIdentical - 1.0f) < 0.001f, "Cosine Similarity of identical vectors is 1.0");

        float simOrthogonal = VectorStore.ComputeCosineSimilarity(vectorA, vectorOrthogonal);
        Assert(Math.Abs(simOrthogonal - 0.0f) < 0.001f, "Cosine Similarity of orthogonal vectors is 0.0");

        float simSimilar = VectorStore.ComputeCosineSimilarity(vectorA, vectorSimilar);
        Assert(simSimilar > 0.90f, "Cosine Similarity of close vectors is high (>0.90)");

        // Test 3: Vector Store In-Memory Ranking
        var mockStore = new VectorStore(null!);
        mockStore.AddChunkWithEmbedding(new VectorDB { Id = "c1", SectionTitle = "Meal", Text = "Meal per diem", Embedding = new float[] { 1.0f, 0.0f } });
        mockStore.AddChunkWithEmbedding(new VectorDB { Id = "c2", SectionTitle = "Leave", Text = "Annual leave", Embedding = new float[] { 0.0f, 1.0f } });

        var results = mockStore.SearchWithVector(new float[] { 0.95f, 0.05f }, topK: 1);
        Assert(results.Count == 1, "VectorStore returns requested top-K count");
        Assert(results[0].Chunk.Id == "c1", "VectorStore returns closest semantic chunk ('Meal')");

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"RESULTS: {passed}/{total} tests passed.");
        Console.WriteLine("==================================================\n");

        return Task.FromResult(passed == total);
    }
}
