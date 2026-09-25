using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGApp_testCases.Evaluation
{
    public class RagEvaluationTestCase
    {
        public string Question { get; set; } = string.Empty;

        // One or more correct chunks
        public List<int> RelevantChunkIds { get; set; } = [];
    }
}
