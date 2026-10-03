using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Embeddings;

namespace VectorDB_RAG
{
    
   
    public class EmbeddingService
    {
        private readonly EmbeddingClient _embeddingClient;

        public EmbeddingService(string apiKey)
        {
            _embeddingClient = new EmbeddingClient(
                model: "text-embedding-3-small",
                apiKey: apiKey);
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            OpenAIEmbedding embedding =
                await _embeddingClient.GenerateEmbeddingAsync(text);

            return embedding.ToFloats().ToArray();
        }
    }
}
