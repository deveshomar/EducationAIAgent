using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI.Chat;
using System.Text.Json;
using RAG_GoldenDataset.Model;
namespace RAG_GoldenDataset.Services
{


    public class RagAnswerEvaluator
    {
        private readonly ChatClient _chatClient;

        public RagAnswerEvaluator(string apiKey)
        {
            _chatClient = new ChatClient(
                model: "gpt-4.1-mini",
                apiKey: apiKey);
        }

        public async Task<AnswerEvaluationResult>
            EvaluateAsync(
                string question,
                string context,
                string expectedAnswer,
                string generatedAnswer)
        {
            string jsonFormat = """
{
  "faithfulnessScore": 0,
  "faithfulnessReason": "",
  "relevanceScore": 0,
  "relevanceReason": "",
  "correctnessScore": 0,
  "correctnessReason": ""
}
""";

            string prompt = $"""
You are a strict evaluator for a RAG system.

Evaluate the generated answer using THREE dimensions.

1. Faithfulness
- Check whether the factual claims in the generated answer
  are supported by the provided context.
- Do not use outside knowledge.
- If the answer contains information that is not supported
  by the context, reduce the score.

2. Relevance
- Check whether the generated answer directly answers
  the user's question.

3. Correctness
- Compare the generated answer with the expected answer.
- Evaluate semantic meaning, not exact wording.
- Different wording is acceptable if the meaning and facts
  are equivalent.

Give each score from 0 to 100.

Scoring:
90-100 = Excellent
70-89 = Mostly correct
50-69 = Partially correct
0-49 = Poor or incorrect

Question:
{question}

Context:
{context}

Expected Answer:
{expectedAnswer}

Generated Answer:
{generatedAnswer}

Return ONLY valid JSON.
Do not include markdown.
Do not include any text before or after the JSON.

Required JSON format:

{jsonFormat}
""";

            ChatCompletion response =
                await _chatClient.CompleteChatAsync(prompt);

            string json = response.Content[0].Text;

            return JsonSerializer.Deserialize<AnswerEvaluationResult>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }
    }
}
