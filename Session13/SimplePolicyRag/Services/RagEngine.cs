using System.Text;
using SimplePolicyRag.Models;
using OpenAI.Chat;

namespace SimplePolicyRag.Services;

public record RagResponse(
    string Answer,
    IReadOnlyList<SearchResult> RetrievedContexts
);

/// <summary>
/// Orchestrates Retrieval-Augmented Generation (RAG): retrieves relevant context chunks from VectorStore,
/// constructs context-augmented prompt, and calls OpenAI ChatClient.
/// </summary>
public class RagEngine
{
    private readonly VectorStore _vectorStore;
    private readonly ChatClient _chatClient;

    public RagEngine(VectorStore vectorStore, ChatClient chatClient)
    {
        _vectorStore = vectorStore;
        _chatClient = chatClient;
    }

    /// <summary>
    /// Executes full RAG pipeline for user question: Retrieve Context -> Augment Prompt -> Generate Answer.
    /// </summary>
    public async Task<RagResponse> AskQuestionAsync(string questionText, int topK = 3, CancellationToken cancellationToken = default)
    {
        // Step 1: Retrieval - Find top-K relevant chunks via Cosine Similarity vector search
        var searchResults = await _vectorStore.SearchAsync(questionText, topK, cancellationToken);

        // Step 2: Augmentation - Build Context String
        var contextBuilder = new StringBuilder();
        contextBuilder.AppendLine("--- OFFICIAL COMPANY POLICY CONTEXT ---");

        if (searchResults.Count == 0)
        {
            contextBuilder.AppendLine("(No matching policy context found)");
        }
        else
        {
            foreach (var res in searchResults)
            {
                contextBuilder.AppendLine($"[Source: {res.Chunk.SectionTitle} | ID: {res.Chunk.Id} | Relevance Score: {res.SimilarityScore:F4}]");
                contextBuilder.AppendLine(res.Chunk.Text);
                contextBuilder.AppendLine();
            }
        }

        // Step 3: Prompt Construction
        string systemPrompt =
            "You are an accurate, helpful Company Policy AI Assistant.\n" +
            "Answer the user's question STRICTLY based on the provided company policy context below.\n" +
            "Rules:\n" +
            "1. Only use facts explicitly mentioned in the context.\n" +
            "2. If the answer is not mentioned in the policy context, say: \"I cannot find that information in the company policy document.\"\n" +
            "3. Include citations to the relevant Section or Chunk ID when stating policy facts.";

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage($"Context Information:\n{contextBuilder}\n\nUser Question: {questionText}")
        };

        // Step 4: Generation - Call OpenAI Chat Client
        ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);
        string answer = completion.Content.Count > 0 ? completion.Content[0].Text : "No response generated.";

        return new RagResponse(answer, searchResults);
    }
}
