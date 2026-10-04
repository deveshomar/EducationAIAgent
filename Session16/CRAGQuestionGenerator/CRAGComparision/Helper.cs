#pragma warning disable OPENAI001

using OpenAI.Embeddings;
using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRAGComparision
{
    public static class Helper
    {
        // ============================================================
        // FUNCTIONS
        // ============================================================

      public  static async Task<float[]> CreateEmbeddingAsync(
            EmbeddingClient client,
            string text)
        {
            var result = await client.GenerateEmbeddingAsync(text);

            return result.Value.ToFloats().ToArray();
        }


        public static double CosineSimilarity(
            float[] vectorA,
            float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException(
                    "Vectors must have the same dimension.");

            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];

                magnitudeA += vectorA[i] * vectorA[i];

                magnitudeB += vectorB[i] * vectorB[i];
            }

            if (magnitudeA == 0 || magnitudeB == 0)
                return 0;

            return dotProduct /
                   (Math.Sqrt(magnitudeA) *
                    Math.Sqrt(magnitudeB));
        }


        public static async Task<string> RewriteQuestionAsync(
            ResponsesClient client,
            string question)
        {
            string prompt = $"""
    You are an HR policy question rewriting engine.

    Rewrite the employee question so that it is easier to retrieve
    the correct HR policy from a knowledge base.

    Rules:
    - Preserve the original intent.
    - Do not answer the question.
    - Do not invent information.
    - Use clear HR terminology.
    - Make the question specific.
    - Return only the rewritten question.

    Employee question:
    {question}
    """;

            ResponseResult response =
                await client.CreateResponseAsync(
                    "gpt-5.2",
                    prompt);

            return response.GetOutputText();
        }


      public  static async Task<string> GenerateAnswerAsync(
            ResponsesClient client,
            string question,
            string context)
        {
            string prompt = $"""
    You are an HR policy assistant.

    Answer the employee's question using ONLY the HR policies
    provided below.

    Rules:
    - Do not invent company policy.
    - If the information is not available, say that the policy
      information provided does not contain the answer.
    - Give a concise answer.
    - Mention the relevant policy when appropriate.

    EMPLOYEE QUESTION:
    {question}

    HR POLICIES:
    {context}
    """;

            ResponseResult response =
                await client.CreateResponseAsync(
                    "gpt-5.2",
                    prompt);

            return response.GetOutputText();
        }


    }
}
