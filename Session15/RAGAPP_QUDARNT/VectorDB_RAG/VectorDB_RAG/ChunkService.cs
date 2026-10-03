using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDB_RAG
{
    public class ChunkService
    {
        public List<DocumentChunk> ReadChunks(string path)
        {
            var text = File.ReadAllText(path);

            var paragraphs = text.Split(
                Environment.NewLine + Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);

            int id = 1;

            return paragraphs
                .Select(x => new DocumentChunk
                {
                    Id = id++,
                    Text = x.Trim()
                })
                .ToList();
        }
    }
}
