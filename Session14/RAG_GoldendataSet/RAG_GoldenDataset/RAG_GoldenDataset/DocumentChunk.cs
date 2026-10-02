using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset
{
  

    public class DocumentChunk
    {
        public string Id { get; set; } = "";
        public string Section { get; set; } = "";
        public string Text { get; set; } = "";

        public float[] Embedding { get; set; } = [];
    }
}
