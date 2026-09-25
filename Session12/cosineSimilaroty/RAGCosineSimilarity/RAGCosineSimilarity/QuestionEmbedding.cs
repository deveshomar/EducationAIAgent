using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGCosineSimilarity
{

    // vector DB
    public class QuestionEmbedding
    {
        public string Question { get; set; } = "";

        public float[] Vector { get; set; } = [];
    }
}
