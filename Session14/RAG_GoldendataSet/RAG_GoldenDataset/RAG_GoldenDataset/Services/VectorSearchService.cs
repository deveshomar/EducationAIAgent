using RAG_GoldenDataset.Model;
using RAG_GoldenDataset.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Services
{


    public class VectorSearchService
    {
        private readonly EmbeddingService _embeddingService;

        public VectorSearchService(
            EmbeddingService embeddingService)
        {
            _embeddingService = embeddingService;
        }

        public async Task<List<SearchResult>> SearchAsync(
            string question,
            List<DocumentChunk> chunks,
            int topK)
        {
            float[] queryEmbedding =
                await _embeddingService
                    .CreateEmbeddingAsync(question);

            var results = chunks
                .Select(chunk => new SearchResult
                {
                    Chunk = chunk,

                    Score = CosineSimilarity.Calculate(
                        queryEmbedding,
                        chunk.Embedding)
                })
                .OrderByDescending(x => x.Score)
                .Take(topK)
                .ToList();

            return results;
        }
    }
}
