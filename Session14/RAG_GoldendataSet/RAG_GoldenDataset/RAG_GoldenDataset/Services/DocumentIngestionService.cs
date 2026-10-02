using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RAG_GoldenDataset.Services
{
   

    public class DocumentIngestionService
    {
        public List<DocumentChunk> LoadAndChunk(string filePath)
        {
            string document = File.ReadAllText(filePath);

            var chunks = new List<DocumentChunk>();

            var matches = Regex.Matches(
                document,
                @"SECTION\s+([A-Z]+-\d+):\s*(.*?)\r?\n={3,}\r?\n([\s\S]*?)(?=\r?\n={3,}\r?\nSECTION|\z)",
                RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                string sectionId = match.Groups[1].Value.Trim();
                string title = match.Groups[2].Value.Trim();
                string body = match.Groups[3].Value.Trim();

                chunks.Add(new DocumentChunk
                {
                    Id = sectionId,
                    Section = title,
                    Text = body
                });
            }

            return chunks;
        }
    }
}
