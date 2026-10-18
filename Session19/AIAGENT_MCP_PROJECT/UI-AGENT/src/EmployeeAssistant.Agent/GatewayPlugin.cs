using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.SemanticKernel;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace EmployeeAssistant.Agent
{
  

    public sealed class GatewayPlugin(McpClient client)
    {
        [KernelFunction("call_backend_tool")]
        [Description(
            "Call an HR or IT backend tool. Choose the exact tool name " +
            "from the available backend tool catalog. Pass its arguments as JSON.")]
        public async Task<string> CallBackendTool(
            [Description("Backend server: hr or it")] string server,
            [Description("Exact backend tool name from the catalog")]
        string toolName,
            [Description("JSON string containing the backend tool arguments")]
        string argumentsJson)
        {
            var arguments =
                JsonSerializer.Deserialize<Dictionary<string, object?>>(
                    argumentsJson) ?? new();

            var result = await client.CallToolAsync(
                "call_backend_tool",
                new Dictionary<string, object?>
                {
                    ["server"] = server,
                    ["toolName"] = toolName,
                    ["arguments"] = arguments
                });

            return string.Join(
                "\n",
                result.Content
                    .OfType<TextContentBlock>()
                    .Select(content => content.Text));
        }
    }
}
