using SimplePolicyRag.Models;

namespace SimplePolicyRag.Services;

/// <summary>
/// Splits raw markdown policy documents into structured text chunks by section headings and paragraphs.
/// </summary>
public static class TextChunkerParser
{
    public static List<VectorDB> ChunkParseDocument(string markdownContent)
    {
        var chunks = new List<VectorDB>();
        var lines = markdownContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        string currentSection = "General Information";
        var currentChunkLines = new List<string>();
        int chunkCounter = 1;

        void FlushChunk()
        {
            if (currentChunkLines.Count > 0)
            {
                string text = string.Join("\n", currentChunkLines).Trim();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    chunks.Add(new VectorDB
                    {
                        Id = $"chunk-{chunkCounter++:D3}",
                        SectionTitle = currentSection,
                        Text = text
                    });
                }
                currentChunkLines.Clear();
            }
        }

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("#"))
            {
                FlushChunk();
                currentSection = trimmedLine.TrimStart('#', ' ').Trim();
            }
            else if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                // Blank line indicates paragraph boundary
                FlushChunk();
            }
            else
            {
                currentChunkLines.Add(trimmedLine);
            }
        }

        FlushChunk();
        return chunks;
    }
}
