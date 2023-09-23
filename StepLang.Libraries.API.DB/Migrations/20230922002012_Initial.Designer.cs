﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StepLang.Libraries.API.DB;

#nullable disable

namespace StepLang.Libraries.API.DB.Migrations
{
    [DbContext(typeof(LibraryApiContext))]
    [Migration("20230922002012_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StepLang.Libraries.API.DB.Entities.Library", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LatestVersionId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LibraryVersionId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LibraryVersionId1")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LatestVersionId")
                        .IsUnique();

                    b.HasIndex("LibraryVersionId");

                    b.HasIndex("LibraryVersionId1");

                    b.ToTable("Libraries");
                });

            modelBuilder.Entity("StepLang.Libraries.API.DB.Entities.LibraryVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LibraryId")
                        .HasColumnType("uuid");

                    b.Property<string>("LibraryJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.ToTable("LibraryVersions");
                });

            modelBuilder.Entity("StepLang.Libraries.API.DB.Entities.Library", b =>
                {
                    b.HasOne("StepLang.Libraries.API.DB.Entities.LibraryVersion", "LatestVersion")
                        .WithOne("Library")
                        .HasForeignKey("StepLang.Libraries.API.DB.Entities.Library", "LatestVersionId");

                    b.HasOne("StepLang.Libraries.API.DB.Entities.LibraryVersion", null)
                        .WithMany("Dependencies")
                        .HasForeignKey("LibraryVersionId");

                    b.HasOne("StepLang.Libraries.API.DB.Entities.LibraryVersion", null)
                        .WithMany("Dependents")
                        .HasForeignKey("LibraryVersionId1");

                    b.Navigation("LatestVersion");
                });

            modelBuilder.Entity("StepLang.Libraries.API.DB.Entities.LibraryVersion", b =>
                {
                    b.HasOne("StepLang.Libraries.API.DB.Entities.Library", null)
                        .WithMany("Versions")
                        .HasForeignKey("LibraryId");
                });

            modelBuilder.Entity("StepLang.Libraries.API.DB.Entities.Library", b =>
                {
                    b.Navigation("Versions");
                });

            modelBuilder.Entity("StepLang.Libraries.API.DB.Entities.LibraryVersion", b =>
                {
                    b.Navigation("Dependencies");

                    b.Navigation("Dependents");

                    b.Navigation("Library")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
