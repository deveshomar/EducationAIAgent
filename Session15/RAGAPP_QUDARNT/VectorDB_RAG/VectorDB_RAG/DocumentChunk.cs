using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDB_RAG
{
   
    public class DocumentChunk
    {
        public int Id { get; set; }

        public string Text { get; set; } = "";

        public float[] Embedding { get; set; } = [];
    }
}
