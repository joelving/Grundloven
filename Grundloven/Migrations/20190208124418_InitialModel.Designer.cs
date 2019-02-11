﻿// <auto-generated />
using System;
using Grundloven.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Grundloven.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190208124418_InitialModel")]
    partial class InitialModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Grundlov.Server.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Grundlov.Server.Models.Article", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<int>("Revision");

                    b.Property<Guid?>("ArticleId");

                    b.Property<int?>("ArticleRevision");

                    b.Property<Guid>("ConstitutionId");

                    b.Property<long>("Created");

                    b.Property<Guid>("SourceId");

                    b.Property<int>("SourceRevision");

                    b.Property<string>("Text");

                    b.HasKey("Id", "Revision");

                    b.HasIndex("ConstitutionId");

                    b.HasIndex("ArticleId", "ArticleRevision");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("Grundlov.Server.Models.ArticleComment", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<int>("Revision");

                    b.Property<Guid?>("ArticleId");

                    b.Property<int?>("ArticleRevision");

                    b.Property<long>("Created");

                    b.Property<string>("Text");

                    b.HasKey("Id", "Revision");

                    b.HasIndex("ArticleId", "ArticleRevision");

                    b.ToTable("ArticleComment");
                });

            modelBuilder.Entity("Grundlov.Server.Models.Constitution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("OwnerId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Constitution");
                });

            modelBuilder.Entity("Grundlov.Server.Models.ConstitutionFollower", b =>
                {
                    b.Property<Guid>("ConstitutionId");

                    b.Property<Guid>("UserId");

                    b.HasKey("ConstitutionId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ConstitutionFollower");
                });

            modelBuilder.Entity("Grundlov.Server.Models.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ArticleId");

                    b.Property<Guid?>("ArticleId1");

                    b.Property<int?>("ArticleRevision");

                    b.Property<long>("Created");

                    b.Property<Guid>("OwnerId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ArticleId1", "ArticleRevision");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Grundlov.Server.Models.Article", b =>
                {
                    b.HasOne("Grundlov.Server.Models.Constitution", "Constitution")
                        .WithMany("Articles")
                        .HasForeignKey("ConstitutionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Grundlov.Server.Models.Article")
                        .WithMany("SubSections")
                        .HasForeignKey("ArticleId", "ArticleRevision");
                });

            modelBuilder.Entity("Grundlov.Server.Models.ArticleComment", b =>
                {
                    b.HasOne("Grundlov.Server.Models.Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId", "ArticleRevision");
                });

            modelBuilder.Entity("Grundlov.Server.Models.Constitution", b =>
                {
                    b.HasOne("Grundlov.Server.Models.ApplicationUser", "Owner")
                        .WithMany("Constitutions")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Grundlov.Server.Models.ConstitutionFollower", b =>
                {
                    b.HasOne("Grundlov.Server.Models.Constitution", "Constitution")
                        .WithMany("Followers")
                        .HasForeignKey("ConstitutionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Grundlov.Server.Models.ApplicationUser", "User")
                        .WithMany("Following")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Grundlov.Server.Models.Like", b =>
                {
                    b.HasOne("Grundlov.Server.Models.ApplicationUser", "Owner")
                        .WithMany("Likes")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Grundlov.Server.Models.Article", "Article")
                        .WithMany("Likes")
                        .HasForeignKey("ArticleId1", "ArticleRevision");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Grundlov.Server.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Grundlov.Server.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Grundlov.Server.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Grundlov.Server.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
