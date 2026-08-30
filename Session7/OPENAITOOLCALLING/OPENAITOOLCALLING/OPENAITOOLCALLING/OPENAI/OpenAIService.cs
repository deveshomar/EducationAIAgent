using OpenAI;
using OpenAI.Chat;
using OPENAITOOLCALLING.TEST;
using OPENAITOOLCALLING.Tools;
using System.Text.Json;

namespace EmployeeAssistant;

public class OpenAIService
{
    private readonly ChatClient _chatClient;
    private readonly EmployeeTools _employeeTools;
    private readonly List<ChatTool> _tools;
    private readonly ToolLogger _toolLogger;
    private List<string> toolresult = new List<string>();
    public OpenAIService()
    {
        string apiKey = "";

        _chatClient = new ChatClient(
            model: "gpt-4.1-mini",
            apiKey: apiKey);

        _employeeTools = new EmployeeTools();

        _tools = ToolDefinitions.GetTools();
        _toolLogger = new ToolLogger();
    }

    public string Ask(string userMessage)
    {
        var chatHistoryMessages = new List<ChatMessage>
        {
            new SystemChatMessage("""
            You are an Employee Assistance AI.

            You help employees with:
            - Salary details
            - Leave balance
            - Leave history
            - Applying leave
            - Tax details
            - Employee details
            - Attendance
            - Manager details
            - Emailing employee information

            Rules:

            1. Use tools whenever the requested information is available through a tool.

            2. If the user asks for information and also asks to email that
               information, first call the required information tool(s),
               then call send_email.

            3. Do not call send_email unless the user explicitly asks
               to send/email the information.

            4. If employee ID is provided, use it.

            5. Never invent employee information.

            6. The send_email tool only requires the recipient email address.
               The application will handle the actual email implementation.
            """),

            new UserChatMessage(userMessage)
        };

        while (true)
        {
            ChatCompletionOptions options = new()
            {
                Temperature = 0
            };

            foreach (var tool in _tools)
            {
                options.Tools.Add(tool);
            }

            //LLM CAll
            ChatCompletion completion =
                _chatClient.CompleteChat(chatHistoryMessages, options);

                string rawJson = JsonSerializer.Serialize(
                completion,
                new JsonSerializerOptions
                {
                WriteIndented = true
                });

            Console.WriteLine(rawJson);


            chatHistoryMessages.Add(new AssistantChatMessage(completion));

            if (completion.FinishReason != ChatFinishReason.ToolCalls)
            {
                string resultOpenai= completion.Content[0].Text;
                new OpenAIService().LogInfo(userMessage, "No Tool Due to Finish Reason STOP ", " ", resultOpenai, 1);
                return resultOpenai;
            }
            ToolExecution obj = new ToolExecution(_employeeTools);
            int executionOrder = 0;

            // validation layer

            //

            // C# code execute 
            foreach (ChatToolCall toolCall in completion.ToolCalls)
            {
                executionOrder++;
                string toolName = toolCall.FunctionName;
                string toolArguments = toolCall.FunctionArguments?.ToString() ?? "";
                string result = obj.ExecuteTool(toolCall, toolresult);
                toolresult.Add(result);
                

                new OpenAIService().LogInfo(userMessage, toolName, toolArguments, result, executionOrder);    

                chatHistoryMessages.Add(
                    new ToolChatMessage(
                        toolCall.Id,
                        result));
            }
        }
    }
    public void LogInfo(string userMessage,string toolName,string toolArguments,string result,int executionOrder)
    {
        _toolLogger.LogToolExecution(
                      userMessage,
                      toolName,
                      toolArguments,
                      result,
                      executionOrder);

    }
    public void RunTaxToolTests()
    {
      var testCases=  TestCases.getTaxTestcases();    

        int total = 0;
        int passed = 0;
        int failed = 0;

        Console.WriteLine();
        Console.WriteLine("======================================================");
        Console.WriteLine("          TAX TOOL SELECTION TEST");
        Console.WriteLine("======================================================");

        foreach (var testCase in testCases)
        {
            total++;

            string actualTool = GetToolFromLLM(testCase.Query);

            bool isPass = string.Equals(
                actualTool,
                testCase.ExpectedTool,
                StringComparison.OrdinalIgnoreCase);

            if (isPass)
                passed++;
            else
                failed++;

            Console.WriteLine();
            Console.WriteLine($"Test #{total}");
            Console.WriteLine($"Query    : {testCase.Query}");
            Console.WriteLine($"Expected : {testCase.ExpectedTool}");
            Console.WriteLine($"Actual   : {actualTool}");
            Console.WriteLine($"Result   : {(isPass ? "PASS " : "FAIL ")}");
        }

        Console.WriteLine();
        Console.WriteLine("======================================================");
        Console.WriteLine("                    SUMMARY");
        Console.WriteLine("======================================================");

        Console.WriteLine($"Total Tests : {total}");
        Console.WriteLine($"Passed      : {passed}");
        Console.WriteLine($"Failed      : {failed}");

        double accuracy = total == 0
            ? 0
            : (double)passed / total * 100;

        Console.WriteLine($"Accuracy    : {accuracy:F2}%");

        Console.WriteLine("======================================================");
    }
    private string GetToolFromLLM(string userMessage)
    {
        var messages = new List<ChatMessage>
    {
        new SystemChatMessage("""
        You are an Employee Assistance AI.

        Use tools whenever the requested information
        is available through a tool.

        Never invent employee information.

        If employee ID is provided, use it.
        """),

        new UserChatMessage(userMessage)
    };

        ChatCompletionOptions options = new()
        {
            Temperature = 0
        };

        foreach (var tool in _tools)
        {
            options.Tools.Add(tool);
        }
        Console.WriteLine($"Query: {userMessage}");
        Console.WriteLine("   ");
        ChatCompletion completion =
            _chatClient.CompleteChat(messages, options);
       
        if (completion.FinishReason != ChatFinishReason.ToolCalls)
        {
            return "NO_TOOL_CALL";
        }

        if (completion.ToolCalls.Count == 0)
        {
            return "NO_TOOL_CALL";
        }

        // For this test we expect one tool
        return completion.ToolCalls[0].FunctionName;
    }

}

/*
 * Please share leave balance for employee 2344
 * Please share salary details for employee 2344
 * Show me leave history for employee 2344
 * Who is the manager of employee 2344?
 * Please share the leave balance and leave history for employee 2344
 * Please email the leave balance details of employee 2344 to devesh.omar@gmail.com
 */ 