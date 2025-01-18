using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "db_metas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    BackgroundImage = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    AppVersion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_metas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "building_assignments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    SegmentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_building_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_building_assignments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "filling_assignments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    SegmentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filling_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_filling_assignments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "matching_assignments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    SegmentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matching_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_matching_assignments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    PdfData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Audio = table.Column<byte[]>(type: "BLOB", nullable: true),
                    SegmentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_materials_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "testing_assignments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    SegmentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testing_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_testing_assignments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    TestingAssignmentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_questions_testing_assignments_TestingAssignmentId",
                        column: x => x.TestingAssignmentId,
                        principalTable: "testing_assignments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "assignment_items",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: true),
                    IsChecked = table.Column<bool>(type: "INTEGER", nullable: false),
                    BuildingAssignmentId = table.Column<string>(type: "TEXT", nullable: true),
                    FillingAssignmentId = table.Column<string>(type: "TEXT", nullable: true),
                    MatchingAssignmentId = table.Column<string>(type: "TEXT", nullable: true),
                    QuestionId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignment_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_assignment_items_building_assignments_BuildingAssignmentId",
                        column: x => x.BuildingAssignmentId,
                        principalTable: "building_assignments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_assignment_items_filling_assignments_FillingAssignmentId",
                        column: x => x.FillingAssignmentId,
                        principalTable: "filling_assignments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_assignment_items_matching_assignments_MatchingAssignmentId",
                        column: x => x.MatchingAssignmentId,
                        principalTable: "matching_assignments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_assignment_items_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "selecting_assignments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<string>(type: "TEXT", nullable: true),
                    SegmentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_selecting_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_selecting_assignments_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_selecting_assignments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_assignment_items_BuildingAssignmentId",
                table: "assignment_items",
                column: "BuildingAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_assignment_items_FillingAssignmentId",
                table: "assignment_items",
                column: "FillingAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_assignment_items_MatchingAssignmentId",
                table: "assignment_items",
                column: "MatchingAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_assignment_items_QuestionId",
                table: "assignment_items",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_building_assignments_SegmentId",
                table: "building_assignments",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_filling_assignments_SegmentId",
                table: "filling_assignments",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_matching_assignments_SegmentId",
                table: "matching_assignments",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_materials_SegmentId",
                table: "materials",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_questions_TestingAssignmentId",
                table: "questions",
                column: "TestingAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_selecting_assignments_QuestionId",
                table: "selecting_assignments",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_selecting_assignments_SegmentId",
                table: "selecting_assignments",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_testing_assignments_SegmentId",
                table: "testing_assignments",
                column: "SegmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assignment_items");

            migrationBuilder.DropTable(
                name: "db_metas");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "selecting_assignments");

            migrationBuilder.DropTable(
                name: "building_assignments");

            migrationBuilder.DropTable(
                name: "filling_assignments");

            migrationBuilder.DropTable(
                name: "matching_assignments");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "testing_assignments");

            migrationBuilder.DropTable(
                name: "Segments");
        }
    }
}
