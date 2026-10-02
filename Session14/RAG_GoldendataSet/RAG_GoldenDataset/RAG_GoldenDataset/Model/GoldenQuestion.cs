using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Model
{


    public class GoldenQuestion
    {
        public int Id { get; set; }

        public string Question { get; set; } = string.Empty;

        public string ExpectedAnswer { get; set; } = string.Empty;

        public List<string> RelevantChunkIds { get; set; } = new();

        public string Category { get; set; } = string.Empty;

        public string Difficulty { get; set; } = string.Empty;
    }
}
