using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FaisalLeague.Data.Access.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leagues_Leagues_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserConnections",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ConnectionId = table.Column<string>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnections", x => new { x.UserId, x.ConnectionId });
                    table.ForeignKey(
                        name: "FK_UserConnections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSeasonLeagues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    SeasonId = table.Column<long>(nullable: false),
                    LeagueId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    SubscriptionDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSeasonLeagues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSeasonLeagues_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSeasonLeagues_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSeasonLeagues_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "IsDeleted", "Name", "Sort" },
                values: new object[,]
                {
                    { 1, false, "اللاذقية", 1 },
                    { 2, false, "دمشق", 2 },
                    { 3, false, "حمص", 3 },
                    { 4, false, "حلب", 4 },
                    { 5, false, "حماه", 5 }
                });

            migrationBuilder.InsertData(
                table: "Leagues",
                columns: new[] { "Id", "Description", "Image", "Name", "ParentId", "Sort" },
                values: new object[,]
                {
                    { 1L, null, "defaultLeague.jpg", "الدوريات", null, 1 },
                    { 6L, null, "defaultLeague.jpg", "الفرق", null, 2 },
                    { 17L, null, "defaultLeague.jpg", "المنتخبات", null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "مدير النظام", "Administrator" });

            migrationBuilder.InsertData(
                table: "Leagues",
                columns: new[] { "Id", "Description", "Image", "Name", "ParentId", "Sort" },
                values: new object[,]
                {
                    { 10L, null, "defaultLeague.jpg", "برشلونة", 6L, 4 },
                    { 18L, null, "defaultLeague.jpg", "انجلترا", 17L, 1 },
                    { 16L, null, "defaultLeague.jpg", "روما", 6L, 10 },
                    { 15L, null, "defaultLeague.jpg", "يوفينتوس", 6L, 9 },
                    { 14L, null, "defaultLeague.jpg", "بايرن ميونخ", 6L, 8 },
                    { 13L, null, "defaultLeague.jpg", "أرسنال", 6L, 7 },
                    { 12L, null, "defaultLeague.jpg", "تشيلسي", 6L, 6 },
                    { 11L, null, "defaultLeague.jpg", "أتلتيكو مدريد", 6L, 5 },
                    { 20L, null, "defaultLeague.jpg", "البرازيل", 17L, 1 },
                    { 9L, null, "defaultLeague.jpg", "ريال مدريد", 6L, 3 },
                    { 8L, null, "defaultLeague.jpg", "مانشستر سيتي", 6L, 2 },
                    { 7L, null, "defaultLeague.jpg", "مانشستر يونايتد", 6L, 1 },
                    { 5L, null, "defaultLeague.jpg", "الألماني", 1L, 4 },
                    { 4L, null, "defaultLeague.jpg", "الانجليزي", 1L, 3 },
                    { 3L, null, "defaultLeague.jpg", "الايطالي", 1L, 2 },
                    { 2L, null, "defaultLeague.jpg", "الاسباني", 1L, 1 },
                    { 19L, null, "defaultLeague.jpg", "فرنسا", 17L, 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CityId", "DOB", "Email", "FirstName", "Image", "IsDeleted", "LastName", "MiddleName", "Mobile", "Password", "Username" },
                values: new object[] { 1, 1, new DateTime(1985, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", "مدير النظام", "defaultUser.jpg", false, "مدير النظام", "مدير النظام", "0933238330", "$2a$11$Y5buKCEzLVmY7XpKdBCoI.dGPlXkZg15tDcbKZTuPRVmt40Yx36Sm", "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_ParentId",
                table: "Leagues",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CityId",
                table: "Users",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSeasonLeagues_LeagueId",
                table: "UserSeasonLeagues",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSeasonLeagues_SeasonId",
                table: "UserSeasonLeagues",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSeasonLeagues_UserId",
                table: "UserSeasonLeagues",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "UserConnections");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserSeasonLeagues");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
