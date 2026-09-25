using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGApp_testCases.Evaluation
{
    public class SampleRagDataSet
    {
        public static List<RagEvaluationTestCase> GetTestData()
        {
            var testCases = new List<RagEvaluationTestCase>
            {
                //new()
                //{
                //    Question="How many annual leaves are allowed?",
                //    RelevantChunkIds=[1,2,3]
                //},
                  new()
                {
                    Question="How many casual leave can person take",
                    RelevantChunkIds=[2]
                },
                // new()
                //{
                //    Question="How many sick leave can person take",
                //    RelevantChunkIds=[3]
                //},
                //new()
                //{
                //    Question="When is salary credited?",
                //    RelevantChunkIds=[5]
                //},
                // new()
                //{
                //    Question="from where i can download my salary slip?",
                //    RelevantChunkIds=[6]
                //},
                // new()
                //{
                //    Question="my mail is not working how i can fix this ",
                //    RelevantChunkIds=[11]
                //},
                //      new()
                //{
                //    Question="can you tell me Travel reimbursement details",
                //    RelevantChunkIds=[14]
                //},
                //new()
                //{
                //    Question="I am facing some issues in laptop how i can fix",
                //    RelevantChunkIds=[9]
                //}
};

            return testCases;
        }
    }
}
