namespace OpenAI_CSharpAPI.Model
{
    public class ChatRequest
    {
        public string Message { get; set; } = "";
    }
    public class ChatResponse
    {
        public string Answer { get; set; } = "";
        public string RawJSON { get; set; } = "";
    }
}
