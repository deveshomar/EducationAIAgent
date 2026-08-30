namespace EmployeeAssistant;

public class ToolLogger
{
    private readonly string _logDirectory;

    public ToolLogger()
    {
        // Console application's running directory
        string baseDirectory = AppContext.BaseDirectory;

        _logDirectory = Path.Combine(baseDirectory, "Logs");

        // Create Logs folder if it doesn't exist
        Directory.CreateDirectory(_logDirectory);
    }

    public void LogToolExecution(
        string userQuery,
        string toolName,
        string toolArguments,
        string toolResult,
        int executionOrder)
    {
        // Create a new file for each day
        string fileName =
            $"tool-execution-{DateTime.Now:yyyy-MM-dd}.txt";

        string logFile =
            Path.Combine(_logDirectory, fileName);

        using StreamWriter writer = new StreamWriter(
            logFile,
            append: true);

        writer.WriteLine("==================================================");
        writer.WriteLine($"Timestamp       : {DateTime.Now}");
        writer.WriteLine($"User Query      : {userQuery}");
        writer.WriteLine($"Execution Order : {executionOrder}");
        writer.WriteLine($"Tool Name       : {toolName}");
        writer.WriteLine($"Tool Arguments  : {toolArguments}");
        writer.WriteLine($"Tool Result     : {toolResult}");
        writer.WriteLine("==================================================");
        writer.WriteLine();
    }
}