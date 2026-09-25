using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGApp.helper
{
    public class Helper
    {
      public  static List<string> SplitText(string text, int chunkSize)
        {
            List<string> chunks = new();

            int current = 0;

            while (current < text.Length)
            {
                int length = Math.Min(chunkSize, text.Length - current);

                chunks.Add(text.Substring(current, length));

                current += chunkSize;
            }

            return chunks;
        }
    }
}
