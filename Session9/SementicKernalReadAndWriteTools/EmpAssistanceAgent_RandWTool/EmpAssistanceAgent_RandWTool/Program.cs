using EmpAssistanceAgent_RandWTool.Approval;
using EmpAssistanceAgent_RandWTool.WriteOperations;
using EmpAssistanceAgent_RandWTool.LLMOperations.ReadOnlyAgent;
using EmpAssistanceAgent_RandWTool.Tools.Read;
using EmpAssistanceAgent_RandWTool.Tools.Write;
using Microsoft.SemanticKernel;

Console.WriteLine("====================================");
Console.WriteLine("      EMPLOYEE AI ASSISTANT");
Console.WriteLine("      Human-in-the-Loop Demo");
Console.WriteLine("====================================");

#region [Open ai key defination]

var apiKey = "";   //Environment.GetEnvironmentVariable("OPENAI_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine(
        "OPENAI_API_KEY environment variable not found.");

    return;
}
#endregion

#region [Define IntentClassifier]
// ============================================
// CREATE INTENT CLASSIFIER KERNEL
// ============================================
var classifierKernelBuilder = Kernel.CreateBuilder();

classifierKernelBuilder.AddOpenAIChatCompletion(
    modelId: "gpt-4.1-mini",
    apiKey: apiKey);

var classifierKernel =
    classifierKernelBuilder.Build();

var intentClassiferAgent =
    new IntentClassifier(classifierKernel);
#endregion 

#region [Define ReadOnlyAgent]

// ============================================
// CREATE READ-ONLY KERNEL
// ============================================

var readKernelBuilder = Kernel.CreateBuilder();

readKernelBuilder.AddOpenAIChatCompletion(
    modelId: "gpt-4.1-mini",
    apiKey: apiKey);

// Only READ tools
readKernelBuilder.Plugins.AddFromType<EmployeeReadTools>();

var readKernel =
    readKernelBuilder.Build();

var readOnlyAgent =  new ReadOnlyAgent(readKernel);
#endregion

#region [Define WriteOperationRouter]  
// ============================================
// CREATE WRITE SERVICES
// ============================================

var writeTools =
    new EmployeeWriteTools();

var policy = new ValidateWriteOperation();

var approval = new HumanApprovalService();

var router = new WriteOperationRouter(writeTools);
#endregion


// ============================================
// EMPLOYEE
// ============================================

var employeeId = "45678";


while (true)
{
    Console.WriteLine();
    Console.WriteLine("------------------------------------");

    Console.Write("Employee > ");

    var userMessage =
        Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userMessage))
        continue;

    if (userMessage.Equals(
        "exit",
        StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    Console.WriteLine();
    Console.WriteLine("Agent processing...");

    try
    {
        #region [Execute intent Classifer]
      
        // ====================================
        // STEP 1: INTENT CLASSIFIER
        // ====================================

        var intentClassiferResponse =
            await intentClassiferAgent.MakeLLMCallsAsync(
                employeeId,
                userMessage);


        Console.WriteLine();
        Console.WriteLine(
            $"Intent: {intentClassiferResponse.Action}");

        #endregion
         
        #region [Validate if Write operations]
      

        // ====================================
        // STEP 2: CHECK WRITE OPERATION
        // ====================================

        if (policy.IsWriteOperation(
            intentClassiferResponse.Action))
        {
            // =================================
            // WRITE OPERATION
            // =================================

            Console.WriteLine();

            Console.WriteLine(
                $"Write operation detected: " +
                $"{intentClassiferResponse.Action}");


            // =================================
            // HUMAN APPROVAL
            // =================================

            var approved =
                approval.RequestApproval(
                    employeeId,
                    intentClassiferResponse.Action,
                    intentClassiferResponse.Arguments);


            if (!approved)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "❌ Operation rejected.");

                continue;
            }


            // =================================
            // EXECUTE WRITE TOOL
            // =================================

            Console.WriteLine();

            Console.WriteLine(
                "✅ Human approved.");

            var toolResult =
                router.Execute(
                    employeeId,
                    intentClassiferResponse.Action,
                    intentClassiferResponse.Arguments);


            Console.WriteLine();

            Console.WriteLine("Tool Result:");

            Console.WriteLine(toolResult);
        }
        #endregion

        #region [Execute Read only agent]
       
        else
        {
            // =================================
            // READ OPERATION
            // =================================

            Console.WriteLine();

            Console.WriteLine( "Sending request to Read-Only Agent...");


            var readResult =
                await readOnlyAgent.MakeLLMCall(
                    
                    userMessage);


            Console.WriteLine();

            Console.WriteLine("Agent Result:");

            Console.WriteLine(readResult);
        }
        #endregion
    }
    catch (Exception ex)
    {
        Console.WriteLine();

        Console.WriteLine(
            $"❌ Error: {ex.Message}");
    }
}