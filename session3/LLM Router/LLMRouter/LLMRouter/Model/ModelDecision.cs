namespace LLMRouter.Model
{
    public class ModelDecision
    {
        public string Model { get; set; } = "";

        public string Reason { get; set; } = "";

        public string Complexity { get; set; } = "";

        public double Confidence { get; set; }
    }
}
