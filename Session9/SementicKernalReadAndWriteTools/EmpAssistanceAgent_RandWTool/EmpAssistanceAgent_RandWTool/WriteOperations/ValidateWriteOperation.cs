using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpAssistanceAgent_RandWTool.WriteOperations
{
    public class ValidateWriteOperation
    {
        private readonly HashSet<string> _writeOperations =
            new(StringComparer.OrdinalIgnoreCase)
            {
            "apply_leave",
            "cancel_leave"
            };

        public bool IsWriteOperation(string action)
        {
            return _writeOperations.Contains(action);
        }
    }
}
