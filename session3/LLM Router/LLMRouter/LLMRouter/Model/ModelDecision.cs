namespace LLMRouter.Model
{
   


    public class ModelSelectionResponse
    {
        public string Model { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Complexity { get; set; } = string.Empty;
        public double Confidence { get; set; }
    }
}
