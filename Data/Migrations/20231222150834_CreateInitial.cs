using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CelebrityWordsQuizes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebrityWordsQuizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbMetas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    AppVersion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbMetas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GapFillerQuizes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GapFillerQuizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProverbBuilderQuizes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProverbBuilderQuizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProverbSelectionQuizes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectQuizId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProverbSelectionQuizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestingQuizItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingQuizItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CelebrityWordsQuizId = table.Column<string>(type: "TEXT", nullable: false),
                    ProverbSelectionQuizId = table.Column<string>(type: "TEXT", nullable: false),
                    ProverbBuilderQuizId = table.Column<string>(type: "TEXT", nullable: false),
                    GapFillerQuizId = table.Column<string>(type: "TEXT", nullable: false),
                    TestingQuizId = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segments_CelebrityWordsQuizes_CelebrityWordsQuizId",
                        column: x => x.CelebrityWordsQuizId,
                        principalTable: "CelebrityWordsQuizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Segments_GapFillerQuizes_GapFillerQuizId",
                        column: x => x.GapFillerQuizId,
                        principalTable: "GapFillerQuizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Segments_ProverbBuilderQuizes_ProverbBuilderQuizId",
                        column: x => x.ProverbBuilderQuizId,
                        principalTable: "ProverbBuilderQuizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Segments_ProverbSelectionQuizes_ProverbSelectionQuizId",
                        column: x => x.ProverbSelectionQuizId,
                        principalTable: "ProverbSelectionQuizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Segments_TestingQuizItems_TestingQuizId",
                        column: x => x.TestingQuizId,
                        principalTable: "TestingQuizItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestingQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectQuizId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    TestingQuizEntityId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestingQuestions_TestingQuizItems_TestingQuizEntityId",
                        column: x => x.TestingQuizEntityId,
                        principalTable: "TestingQuizItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ListeningMaterials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Audio = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    SegmentEntityId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListeningMaterials_Segments_SegmentEntityId",
                        column: x => x.SegmentEntityId,
                        principalTable: "Segments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReadingMaterials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false),
                    SegmentEntityId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingMaterials_Segments_SegmentEntityId",
                        column: x => x.SegmentEntityId,
                        principalTable: "Segments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuizItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CelebrityWordsQuizEntityId = table.Column<string>(type: "TEXT", nullable: true),
                    GapFillerQuizEntityId = table.Column<string>(type: "TEXT", nullable: true),
                    ProverbBuilderQuizEntityId = table.Column<string>(type: "TEXT", nullable: true),
                    ProverbSelectionQuizEntityId = table.Column<string>(type: "TEXT", nullable: true),
                    TestingQuestionEntityId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizItems_CelebrityWordsQuizes_CelebrityWordsQuizEntityId",
                        column: x => x.CelebrityWordsQuizEntityId,
                        principalTable: "CelebrityWordsQuizes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizItems_GapFillerQuizes_GapFillerQuizEntityId",
                        column: x => x.GapFillerQuizEntityId,
                        principalTable: "GapFillerQuizes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizItems_ProverbBuilderQuizes_ProverbBuilderQuizEntityId",
                        column: x => x.ProverbBuilderQuizEntityId,
                        principalTable: "ProverbBuilderQuizes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizItems_ProverbSelectionQuizes_ProverbSelectionQuizEntityId",
                        column: x => x.ProverbSelectionQuizEntityId,
                        principalTable: "ProverbSelectionQuizes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizItems_TestingQuestions_TestingQuestionEntityId",
                        column: x => x.TestingQuestionEntityId,
                        principalTable: "TestingQuestions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListeningMaterials_SegmentEntityId",
                table: "ListeningMaterials",
                column: "SegmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItems_CelebrityWordsQuizEntityId",
                table: "QuizItems",
                column: "CelebrityWordsQuizEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItems_GapFillerQuizEntityId",
                table: "QuizItems",
                column: "GapFillerQuizEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItems_ProverbBuilderQuizEntityId",
                table: "QuizItems",
                column: "ProverbBuilderQuizEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItems_ProverbSelectionQuizEntityId",
                table: "QuizItems",
                column: "ProverbSelectionQuizEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizItems_TestingQuestionEntityId",
                table: "QuizItems",
                column: "TestingQuestionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingMaterials_SegmentEntityId",
                table: "ReadingMaterials",
                column: "SegmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_CelebrityWordsQuizId",
                table: "Segments",
                column: "CelebrityWordsQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_GapFillerQuizId",
                table: "Segments",
                column: "GapFillerQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_ProverbBuilderQuizId",
                table: "Segments",
                column: "ProverbBuilderQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_ProverbSelectionQuizId",
                table: "Segments",
                column: "ProverbSelectionQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_TestingQuizId",
                table: "Segments",
                column: "TestingQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingQuestions_TestingQuizEntityId",
                table: "TestingQuestions",
                column: "TestingQuizEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbMetas");

            migrationBuilder.DropTable(
                name: "ListeningMaterials");

            migrationBuilder.DropTable(
                name: "QuizItems");

            migrationBuilder.DropTable(
                name: "ReadingMaterials");

            migrationBuilder.DropTable(
                name: "TestingQuestions");

            migrationBuilder.DropTable(
                name: "Segments");

            migrationBuilder.DropTable(
                name: "CelebrityWordsQuizes");

            migrationBuilder.DropTable(
                name: "GapFillerQuizes");

            migrationBuilder.DropTable(
                name: "ProverbBuilderQuizes");

            migrationBuilder.DropTable(
                name: "ProverbSelectionQuizes");

            migrationBuilder.DropTable(
                name: "TestingQuizItems");
        }
    }
}
