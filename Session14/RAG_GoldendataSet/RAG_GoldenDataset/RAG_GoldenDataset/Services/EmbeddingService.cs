using OpenAI.Embeddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Services

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

        public async Task<float[]> CreateEmbeddingAsync(string text)
        {
            var embedding =
                await _client.GenerateEmbeddingAsync(text);

            return embedding.Value.ToFloats().ToArray();
        }
    }
}
