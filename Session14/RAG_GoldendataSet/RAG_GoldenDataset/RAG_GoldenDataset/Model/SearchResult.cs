using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Model
{
    public class SearchResult
    {
        public DocumentChunk Chunk { get; set; } = null!;

        public double Score { get; set; }
    }
}
