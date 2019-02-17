using Grundloven.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using System;

namespace Grundloven.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var instantConversion = new ValueConverter<Instant, long>(
                v => v.ToUnixTimeMilliseconds(),
                v => Instant.FromUnixTimeMilliseconds(v)
            );
            
            builder.Entity<Article>()
                .Property(e => e.Created).HasConversion(instantConversion);
            
            builder.Entity<ArticleComment>()
                .Property(e => e.Created).HasConversion(instantConversion);

            builder.Entity<Like>()
                .Property(e => e.Created).HasConversion(instantConversion);

            builder.Entity<ConstitutionFollower>()
                .HasKey(a => new { a.ConstitutionId, a.UserId });


            // Cascades.
            builder.Entity<ApplicationUser>()
                .HasMany(c => c.Constitutions)
                .WithOne(f => f.Owner);
            builder.Entity<Constitution>()
                .HasOne(c => c.Owner)
                .WithMany(f => f.Constitutions);

            builder.Entity<ApplicationUser>()
                .HasMany(c => c.Following)
                .WithOne(f => f.User);
            builder.Entity<ConstitutionFollower>()
                .HasOne(p => p.User)
                .WithMany(p => p.Following)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Constitution>()
                .HasMany(c => c.Followers)
                .WithOne(f => f.Constitution);
            builder.Entity<ConstitutionFollower>()
                .HasOne(p => p.Constitution)
                .WithMany(p => p.Followers)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Article>()
                .HasOne(c => c.Source)
                .WithMany(f => f.Revisions)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Article>()
                .HasMany(c => c.Revisions)
                .WithOne(f => f.Source)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Article>()
                .HasMany(c => c.SubSections)
                .WithOne(f => f.Parent);
            builder.Entity<Article>()
                .HasOne(c => c.Parent)
                .WithMany(f => f.SubSections)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Article>()
                .HasOne(c => c.Constitution)
                .WithMany(f => f.Articles)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Constitution>()
                .HasMany(c => c.Articles)
                .WithOne(f => f.Constitution);

            builder.Entity<Article>()
                .HasMany(c => c.Likes)
                .WithOne(f => f.Article);
            builder.Entity<Like>()
                .HasOne(c => c.Article)
                .WithMany(f => f.Likes)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasOne(c => c.Owner)
                .WithMany(f => f.Likes)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationUser>()
                .HasMany(c => c.Likes)
                .WithOne(f => f.Owner);

            builder.Entity<Article>()
                .HasMany(c => c.Comments)
                .WithOne(f => f.Article);
            builder.Entity<ArticleComment>()
                .HasOne(c => c.Article)
                .WithMany(f => f.Comments)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ArticleComment>()
                .HasOne(c => c.Source)
                .WithOne(f => f.Revision)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ArticleComment>()
                .HasOne(c => c.Revision)
                .WithOne(f => f.Source)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
