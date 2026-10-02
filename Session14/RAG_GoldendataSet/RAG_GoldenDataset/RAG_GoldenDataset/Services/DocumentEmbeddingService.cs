using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Services
{
 

    public class DocumentEmbeddingService
    {
        private readonly EmbeddingService _embeddingService;

        public DocumentEmbeddingService(
            EmbeddingService embeddingService)
        {
            _embeddingService = embeddingService;
        }

        public async Task CreateEmbeddingsAsync(
            List<DocumentChunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                Console.WriteLine(
                    $"Embedding {chunk.Id}...");

                chunk.Embedding =
                    await _embeddingService
                        .CreateEmbeddingAsync(chunk.Text);
            }
        }
    }
}
