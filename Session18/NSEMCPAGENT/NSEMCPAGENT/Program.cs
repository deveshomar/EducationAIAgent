
using System;
using System.Linq;
using System.Threading.Tasks;

using ModelContextProtocol.Client;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable SKEXP0001

class Program
{
    static async Task Main()
    {
        // 1. Read OpenAI API key

        var apiKey = "sk-proj-5k6E-nkA";


        // 2. Connect to the public NSE MCP server
        var transport = new HttpClientTransport(
            new HttpClientTransportOptions
            {
                Endpoint = new Uri(
                    "https://mcp.nseindia.in/bhavcopy/cm/mcp")
            });

        await using var mcpClient =
            await McpClient.CreateAsync(transport);

        Console.WriteLine("Connected to NSE MCP server.");

        // 3. Discover tools exposed by the NSE MCP server
        var mcpTools = await mcpClient.ListToolsAsync();

        Console.WriteLine("\nAvailable NSE tools:");

        foreach (var tool in mcpTools)
        {
            Console.WriteLine(
                $"- {tool.Name}: {tool.Description}");
        }

        // 4. Create Semantic Kernel
        var builder = Kernel.CreateBuilder();

        builder.AddOpenAIChatCompletion(
            modelId: "gpt-4.1-mini",
            apiKey: apiKey);

        var kernel = builder.Build();


        //  21 tools llm
        // 5. Register discovered MCP tools as SK functions
        kernel.Plugins.AddFromFunctions(
            "NSE",
            mcpTools.Select(tool => tool.AsKernelFunction()));

        Console.WriteLine(
            $"\nRegistered {mcpTools.Count} NSE tools.");


        //funcaiton call is auto

        // 6. Enable automatic function calling
        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior =
                FunctionChoiceBehavior.Auto(
                    options: new()
                    {
                        RetainArgumentTypes = true
                    })
        };

        var chatService =
            kernel.GetRequiredService<IChatCompletionService>();

        // 7. Chat loop
        var history = new ChatHistory();

        history.AddSystemMessage(""""
            
            history.AddSystemMessage("""
                You are an NSE (National Stock Exchange of India)
                market-data AI assistant.

                You have access to tools provided by the NSE MCP server.
                Use these tools to retrieve current market information.
                Never invent stock prices, index values, gainers, losers,
                bond details, or data freshness information.

                TOOL CAPABILITIES AND WHEN TO USE THEM

                1. LIVE TOP GAINERS
                   When the user asks for top gainers, stocks gaining the
                   most, or best-performing stocks in the available market
                   data, use the live top gainers tool.

                2. LIVE MARKET DATA BY INDEX
                   When the user asks about NIFTY, BANK NIFTY, or another
                   supported market index, use the index market-data tool.
                   Report only the indices and fields returned by the tool.

                3. SME SEGMENT STOCKS
                   When the user asks about SME stocks, SME listings, or
                   the SME segment, use the SME stocks tool.

                4. INDIVIDUAL STOCK QUOTE
                   When the user asks for the current quote, price, or
                   available market details of a particular stock, use
                   the individual stock quote tool.
                   Ask for the stock symbol or company name if unclear.

                5. LIVE TOP LOSERS
                   When the user asks for top losers, biggest declines,
                   or worst-performing stocks in the available market data,
                   use the live top losers tool.

                6. ALL EQUITY STOCKS LISTING
                   When the user asks for all listed equity stocks, stock
                   listings, or available equity symbols, use the equity
                   stocks listing tool.
                   If the result is large, summarize it or ask which
                   segment or subset the user wants.

                7. BOND LISTINGS
                   When the user asks about listed bonds or available bond
                   listings, use the bond listings tool.
                   Do not assume every bond is listed or tradable.

                8. DATA FRESHNESS STATUS
                   When the user asks whether the data is live, when it
                   was last updated, whether data is stale, or how fresh
                   the market information is, use the data freshness tool.
                   If relevant, check freshness before presenting data
                   as current.

                GENERAL RULES

                - Select tools based on the user's intent.
                - Use actual tool results as the source of market facts.
                - Do not fabricate missing fields or unsupported values.
                - Clearly distinguish the latest available data from
                  real-time data; do not claim data is live unless the
                  tool results support that claim.
                - Include timestamps and units when returned.
                - If a tool fails, explain that the data could not be
                  retrieved instead of making up an answer.
                - For comparisons, retrieve the relevant data for each
                  requested stock or index when the tools support it.
                - For questions outside these capabilities, explain
                  the limitation rather than inventing MCP functionality.
                - Provide clear, concise answers using tables when
                  presenting multiple stocks or indices.
                - Market data is informational, not a guarantee of
                  future performance or investment advice.
                """);
            
            """");

        while (true)
        {
            Console.Write("\nYou: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Equals(
                "exit",
                StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            history.AddUserMessage(input);

            // 8. LLM selects and invokes NSE tools as needed
            var response =
                await chatService.GetChatMessageContentAsync(
                    history,
                    executionSettings: settings,
                    kernel: kernel);

            string answer = response.Content ?? "";

            history.AddAssistantMessage(answer);

            Console.WriteLine($"\nAgent: {answer}");
        }
    }
}

#pragma warning restore SKEXP0001
