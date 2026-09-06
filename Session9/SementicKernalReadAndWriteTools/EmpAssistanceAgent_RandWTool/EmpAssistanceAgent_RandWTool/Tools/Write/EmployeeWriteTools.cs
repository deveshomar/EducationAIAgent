using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpAssistanceAgent_RandWTool.Tools.Write
{
    public class EmployeeWriteTools
    {
        public string ApplyLeave(
            string employeeId,
            string fromDate,
            string toDate,
            string reason)
        {
            Console.WriteLine();
            Console.WriteLine("[WRITE TOOL] ApplyLeave executing...");

            // Normally database operation
            if(employeeId == null || fromDate == null || toDate == null || reason == null)
            {
                throw new ArgumentNullException("Employee ID, From Date, To Date, and Reason cannot be null.");
            }   
            if(employeeId !=null && int.Parse( employeeId)<0 )
            {
                throw new ArgumentNullException("Invalid emp ID.");
            }     
            if(DateTime.Parse(fromDate) > DateTime.Parse(toDate))
            {
                throw new ArgumentException("From Date cannot be later than To Date.");
            }   
            if(string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Reason cannot be empty.");
            }    
            if(DateTime.Parse(fromDate) < DateTime.Now)
            {
                throw new ArgumentException("From Date cannot be in the past.");
            }   
            if(DateTime.Parse(toDate) < DateTime.Now)
            {
                throw new ArgumentException("To Date cannot be in the past.");
            }  
            if((DateTime.Parse(toDate) - DateTime.Parse(fromDate)).TotalDays > 30)
            {
                throw new ArgumentException("Leave duration cannot exceed 30 days.");
            }   
            return
                $"Leave successfully applied for {employeeId} " +
                $"from {fromDate} to {toDate}.";
        }

        public string CancelLeave(
            string employeeId,
            string leaveId)
        {
            if(employeeId == null || leaveId == null)
            {
                throw new ArgumentNullException("Employee ID and Leave ID cannot be null.");
            }
            if (employeeId !=null && int.Parse( employeeId)<0 )
            {
                throw new ArgumentNullException("Invalid emp ID.");
            }
            Console.WriteLine();
            Console.WriteLine("[WRITE TOOL] CancelLeave executing...");

            // Normally database operation

            return
                $"Leave {leaveId} successfully cancelled.";
        }
    }
}
