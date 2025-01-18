﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250118182023_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("Data.Entities.BuildingAssignment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("SegmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.ToTable("building_assignments");
                });

            modelBuilder.Entity("Data.Entities.DbMeta", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AppVersion")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("BackgroundImage")
                        .HasColumnType("BLOB");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("db_metas");
                });

            modelBuilder.Entity("Data.Entities.FillingAssignment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("SegmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.ToTable("filling_assignments");
                });

            modelBuilder.Entity("Data.Entities.MatchingAssignment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("SegmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.ToTable("matching_assignments");
                });

            modelBuilder.Entity("Data.Entities.Material", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Audio")
                        .HasColumnType("BLOB");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PdfData")
                        .HasColumnType("BLOB");

                    b.Property<string>("SegmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.ToTable("materials");
                });

            modelBuilder.Entity("Data.Entities.Segment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("Data.Entities.SelectingAssignment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("QuestionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SegmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("SegmentId");

                    b.ToTable("selecting_assignments");
                });

            modelBuilder.Entity("Data.Entities.TaskItems.AssignmentItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BuildingAssignmentId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("FillingAssignmentId")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Image")
                        .HasColumnType("BLOB");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MatchingAssignmentId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("QuestionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BuildingAssignmentId");

                    b.HasIndex("FillingAssignmentId");

                    b.HasIndex("MatchingAssignmentId");

                    b.HasIndex("QuestionId");

                    b.ToTable("assignment_items");
                });

            modelBuilder.Entity("Data.Entities.TaskItems.Question", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("TestingAssignmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TestingAssignmentId");

                    b.ToTable("questions");
                });

            modelBuilder.Entity("Data.Entities.TestingAssignment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("SegmentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.ToTable("testing_assignments");
                });

            modelBuilder.Entity("Data.Entities.BuildingAssignment", b =>
                {
                    b.HasOne("Data.Entities.Segment", null)
                        .WithMany("BuildingAssignments")
                        .HasForeignKey("SegmentId");
                });

            modelBuilder.Entity("Data.Entities.FillingAssignment", b =>
                {
                    b.HasOne("Data.Entities.Segment", null)
                        .WithMany("FillingAssignments")
                        .HasForeignKey("SegmentId");
                });

            modelBuilder.Entity("Data.Entities.MatchingAssignment", b =>
                {
                    b.HasOne("Data.Entities.Segment", null)
                        .WithMany("MatchingAssignments")
                        .HasForeignKey("SegmentId");
                });

            modelBuilder.Entity("Data.Entities.Material", b =>
                {
                    b.HasOne("Data.Entities.Segment", null)
                        .WithMany("Materials")
                        .HasForeignKey("SegmentId");
                });

            modelBuilder.Entity("Data.Entities.SelectingAssignment", b =>
                {
                    b.HasOne("Data.Entities.TaskItems.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");

                    b.HasOne("Data.Entities.Segment", null)
                        .WithMany("SelectionAssignments")
                        .HasForeignKey("SegmentId");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Data.Entities.TaskItems.AssignmentItem", b =>
                {
                    b.HasOne("Data.Entities.BuildingAssignment", null)
                        .WithMany("Items")
                        .HasForeignKey("BuildingAssignmentId");

                    b.HasOne("Data.Entities.FillingAssignment", null)
                        .WithMany("Items")
                        .HasForeignKey("FillingAssignmentId");

                    b.HasOne("Data.Entities.MatchingAssignment", null)
                        .WithMany("Items")
                        .HasForeignKey("MatchingAssignmentId");

                    b.HasOne("Data.Entities.TaskItems.Question", null)
                        .WithMany("Options")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("Data.Entities.TaskItems.Question", b =>
                {
                    b.HasOne("Data.Entities.TestingAssignment", null)
                        .WithMany("Questions")
                        .HasForeignKey("TestingAssignmentId");
                });

            modelBuilder.Entity("Data.Entities.TestingAssignment", b =>
                {
                    b.HasOne("Data.Entities.Segment", null)
                        .WithMany("TestingAssignments")
                        .HasForeignKey("SegmentId");
                });

            modelBuilder.Entity("Data.Entities.BuildingAssignment", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Data.Entities.FillingAssignment", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Data.Entities.MatchingAssignment", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Data.Entities.Segment", b =>
                {
                    b.Navigation("BuildingAssignments");

                    b.Navigation("FillingAssignments");

                    b.Navigation("MatchingAssignments");

                    b.Navigation("Materials");

                    b.Navigation("SelectionAssignments");

                    b.Navigation("TestingAssignments");
                });

            modelBuilder.Entity("Data.Entities.TaskItems.Question", b =>
                {
                    b.Navigation("Options");
                });

            modelBuilder.Entity("Data.Entities.TestingAssignment", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
