using NodaTime;
using System;

namespace Grundloven.Models
{
    public class ArticleComment
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }

        public string Text { get; set; }
        public Instant Created { get; set; }
    }
}