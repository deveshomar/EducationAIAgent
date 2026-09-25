using OpenAI.Embeddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGApp.embeddings
{
    public class EmbeddingService
    {
        private readonly EmbeddingClient _client;

        public EmbeddingService(string apiKey)
        {
            _client = new EmbeddingClient(
                "text-embedding-3-small",
                apiKey);
        }

        public async Task<float[]> CreateEmbedding(string text)
        {
            var result = await _client.GenerateEmbeddingAsync(text);

            return result.Value.ToFloats().ToArray();
        }
    }
}
