namespace SimplePolicyRag.Models;

/// <summary>
/// Represents a text chunk from a document along with its vector embedding.
/// </summary>
public class VectorDB
{
    public string Id { get; set; } = string.Empty;
    public string SectionTitle { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public float[] Embedding { get; set; } = Array.Empty<float>();
}
