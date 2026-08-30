using EmployeeAssistant;


var service = new OpenAIService();

Console.WriteLine("Employee Assistant");
Console.WriteLine("==================");

while (true)
{
    Console.Write("\nYou: ");

    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Equals("exit",
        StringComparison.OrdinalIgnoreCase))
    {
        break;
    }


     //string response = service.Ask(input);

    if (input.Equals("Execute tests", StringComparison.OrdinalIgnoreCase))
    {
         service.RunTaxToolTests();
        //service.RunTaxToolTests();

    }


   // Console.WriteLine($"\nAI: {response}");
}