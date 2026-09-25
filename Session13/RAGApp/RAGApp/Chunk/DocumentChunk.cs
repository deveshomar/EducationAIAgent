using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGApp.Chunk
{
    public class DocumentChunk
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        // We will populate this in the next step
        public float[]? Embedding { get; set; }
    }
}
