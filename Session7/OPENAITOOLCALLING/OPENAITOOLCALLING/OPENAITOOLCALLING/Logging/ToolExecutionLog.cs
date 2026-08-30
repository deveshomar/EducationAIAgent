using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPENAITOOLCALLING.Logging
{
    public class ToolExecutionLog
    {
        public string UserQuery { get; set; } = "";
        public string ToolName { get; set; } = "";
        public string ToolArguments { get; set; } = "";
        public string ToolResult { get; set; } = "";
        public int ExecutionOrder { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
