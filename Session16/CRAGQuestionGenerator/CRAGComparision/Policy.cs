using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRAGComparision
{

    // ============================================================
    // MODEL
    // ============================================================

    class Policy
    {
        public int Id { get; }

        public string Title { get; }

        public string Content { get; }

        public float[]? Embedding { get; set; }

        public Policy(
            int id,
            string title,
            string content)
        {
            Id = id;
            Title = title;
            Content = content;
        }
    }
}
