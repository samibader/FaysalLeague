using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FaisalLeague.Data.Access.Migrations
{
    public partial class Edit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    UserCardId = table.Column<long>(nullable: false),
                    QuestionId = table.Column<long>(nullable: false),
                    ChoiceId = table.Column<long>(nullable: true),
                    AnswerPoints = table.Column<int>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => new { x.UserCardId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_UserAnswers_Choices_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_UserCards_UserCardId",
                        column: x => x.UserCardId,
                        principalTable: "UserCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_ChoiceId",
                table: "UserAnswers",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAnswers");
        }
    }
}
