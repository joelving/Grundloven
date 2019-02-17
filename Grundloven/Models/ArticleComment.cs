using NodaTime;
using System;

namespace Grundloven.Models
{
    public class ArticleComment
    {
        public Guid Id { get; set; }

        public Guid? SourceId { get; set; }
        public ArticleComment Source { get; set; }

        public Guid? RevisionId { get; set; }
        public ArticleComment Revision { get; set; }

        public Guid ArticleId { get; set; }
        public Article Article { get; set; }

        public string Text { get; set; }
        public Instant Created { get; set; }
    }
}