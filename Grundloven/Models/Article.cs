using NodaTime;
using System;
using System.Collections.Generic;

namespace Grundloven.Models
{
    public class Article
    {
        public Guid Id { get; set; }

        public Guid? SourceId { get; set; }
        public Article Source { get; set; }
        public ICollection<Article> Revisions { get; set; }

        public string Text { get; set; }
        public Instant Created { get; set; }

        public Guid ConstitutionId { get; set; }
        public Constitution Constitution { get; set; }

        public Guid? ParentId { get; set; }
        public Article Parent { get; set; }
        public ICollection<Article> SubSections { get; set; }
        
        public ICollection<ArticleComment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
