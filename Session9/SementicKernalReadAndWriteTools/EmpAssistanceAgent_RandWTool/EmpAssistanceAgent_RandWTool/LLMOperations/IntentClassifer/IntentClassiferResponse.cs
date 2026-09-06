using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpAssistanceAgent_RandWTool.LLMOperations.IntentClassifer
{
    public class IntentClassiferResponse
    {
        public string Action { get; set; } = "";

        public Dictionary<string, string> Arguments { get; set; }
            = new();

        public string Reason { get; set; } = "";
    }
}
