﻿// <auto-generated />
using System;
using Leap.API.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Leap.API.DB.Migrations
{
    [DbContext(typeof(LeapApiDbContext))]
    partial class LibraryApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuthorLibrary", b =>
                {
                    b.Property<Guid>("LibrariesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MaintainersId")
                        .HasColumnType("uuid");

                    b.HasKey("LibrariesId", "MaintainersId");

                    b.HasIndex("MaintainersId");

                    b.ToTable("AuthorLibrary");
                });

            modelBuilder.Entity("Leap.API.DB.Entities.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Leap.API.DB.Entities.Library", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("LatestVersionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LatestVersionId")
                        .IsUnique();

                    b.ToTable("Libraries");
                });

            modelBuilder.Entity("Leap.API.DB.Entities.LibraryVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LibraryVersions");
                });

            modelBuilder.Entity("Leap.API.DB.Entities.LibraryVersionDependency", b =>
                {
                    b.Property<Guid>("DependencyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("VersionId")
                        .HasColumnType("uuid");

                    b.HasKey("DependencyId", "VersionId");

                    b.HasIndex("VersionId");

                    b.ToTable("LibraryVersionDependency");
                });

            modelBuilder.Entity("AuthorLibrary", b =>
                {
                    b.HasOne("Leap.API.DB.Entities.Library", null)
                        .WithMany()
                        .HasForeignKey("LibrariesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Leap.API.DB.Entities.Author", null)
                        .WithMany()
                        .HasForeignKey("MaintainersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Leap.API.DB.Entities.Library", b =>
                {
                    b.HasOne("Leap.API.DB.Entities.LibraryVersion", "LatestVersion")
                        .WithOne("Library")
                        .HasForeignKey("Leap.API.DB.Entities.Library", "LatestVersionId");

                    b.Navigation("LatestVersion");
                });

            modelBuilder.Entity("Leap.API.DB.Entities.LibraryVersionDependency", b =>
                {
                    b.HasOne("Leap.API.DB.Entities.Library", "Dependency")
                        .WithMany()
                        .HasForeignKey("DependencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Leap.API.DB.Entities.LibraryVersion", "Version")
                        .WithMany()
                        .HasForeignKey("VersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dependency");

                    b.Navigation("Version");
                });

            modelBuilder.Entity("Leap.API.DB.Entities.LibraryVersion", b =>
                {
                    b.Navigation("Library")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
