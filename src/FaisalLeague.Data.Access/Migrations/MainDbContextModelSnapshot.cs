﻿// <auto-generated />
using System;
using FaisalLeague.Data.Access.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FaisalLeague.Data.Access.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FaisalLeague.Data.Model.Card", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.CardQuestion", b =>
                {
                    b.Property<long>("CardId");

                    b.Property<long>("QuestionId");

                    b.Property<int>("Sort");

                    b.HasKey("CardId", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("CardQuestions");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.CardState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("CardStates");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("Sort");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.Choice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsCorrect");

                    b.Property<long>("QuestionId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Choices");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<int>("Sort");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new { Id = 1, IsDeleted = false, Name = "اللاذقية", Sort = 1 },
                        new { Id = 2, IsDeleted = false, Name = "دمشق", Sort = 2 },
                        new { Id = 3, IsDeleted = false, Name = "حمص", Sort = 3 },
                        new { Id = 4, IsDeleted = false, Name = "حلب", Sort = 4 },
                        new { Id = 5, IsDeleted = false, Name = "حماه", Sort = 5 }
                    );
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.League", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<long?>("ParentId");

                    b.Property<int>("Sort");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Leagues");

                    b.HasData(
                        new { Id = 1L, Image = "defaultLeague.jpg", Name = "الدوريات", Sort = 1 },
                        new { Id = 2L, Image = "defaultLeague.jpg", Name = "الاسباني", ParentId = 1L, Sort = 1 },
                        new { Id = 3L, Image = "defaultLeague.jpg", Name = "الايطالي", ParentId = 1L, Sort = 2 },
                        new { Id = 4L, Image = "defaultLeague.jpg", Name = "الانجليزي", ParentId = 1L, Sort = 3 },
                        new { Id = 5L, Image = "defaultLeague.jpg", Name = "الألماني", ParentId = 1L, Sort = 4 },
                        new { Id = 6L, Image = "defaultLeague.jpg", Name = "الفرق", Sort = 2 },
                        new { Id = 7L, Image = "defaultLeague.jpg", Name = "مانشستر يونايتد", ParentId = 6L, Sort = 1 },
                        new { Id = 8L, Image = "defaultLeague.jpg", Name = "مانشستر سيتي", ParentId = 6L, Sort = 2 },
                        new { Id = 9L, Image = "defaultLeague.jpg", Name = "ريال مدريد", ParentId = 6L, Sort = 3 },
                        new { Id = 10L, Image = "defaultLeague.jpg", Name = "برشلونة", ParentId = 6L, Sort = 4 },
                        new { Id = 11L, Image = "defaultLeague.jpg", Name = "أتلتيكو مدريد", ParentId = 6L, Sort = 5 },
                        new { Id = 12L, Image = "defaultLeague.jpg", Name = "تشيلسي", ParentId = 6L, Sort = 6 },
                        new { Id = 13L, Image = "defaultLeague.jpg", Name = "أرسنال", ParentId = 6L, Sort = 7 },
                        new { Id = 14L, Image = "defaultLeague.jpg", Name = "بايرن ميونخ", ParentId = 6L, Sort = 8 },
                        new { Id = 15L, Image = "defaultLeague.jpg", Name = "يوفينتوس", ParentId = 6L, Sort = 9 },
                        new { Id = 16L, Image = "defaultLeague.jpg", Name = "روما", ParentId = 6L, Sort = 10 },
                        new { Id = 17L, Image = "defaultLeague.jpg", Name = "المنتخبات", Sort = 3 },
                        new { Id = 18L, Image = "defaultLeague.jpg", Name = "انجلترا", ParentId = 17L, Sort = 1 },
                        new { Id = 19L, Image = "defaultLeague.jpg", Name = "فرنسا", ParentId = 17L, Sort = 1 },
                        new { Id = 20L, Image = "defaultLeague.jpg", Name = "البرازيل", ParentId = 17L, Sort = 1 }
                    );
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoryId");

                    b.Property<string>("DecryptionKey");

                    b.Property<string>("Text");

                    b.Property<string>("VideoCode");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.QuestionPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.ToTable("QuestionPoints");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new { Id = 1, Description = "مدير النظام", Name = "Administrator" }
                    );
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.Season", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<DateTime>("DOB");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Image");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Mobile");

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Users");

                    b.HasData(
                        new { Id = 1, CityId = 1, DOB = new DateTime(1985, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), Email = "admin@admin.com", FirstName = "مدير النظام", Image = "defaultUser.jpg", IsDeleted = false, LastName = "مدير النظام", MiddleName = "مدير النظام", Mobile = "0933238330", Password = "$2a$11$Y5buKCEzLVmY7XpKdBCoI.dGPlXkZg15tDcbKZTuPRVmt40Yx36Sm", Username = "admin" }
                    );
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserAnswer", b =>
                {
                    b.Property<long>("UserCardId");

                    b.Property<long>("QuestionId");

                    b.Property<int>("AnswerPoints");

                    b.Property<long?>("ChoiceId");

                    b.Property<int>("Sort");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("UserCardId", "QuestionId");

                    b.HasIndex("ChoiceId");

                    b.HasIndex("QuestionId");

                    b.ToTable("UserAnswers");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserCard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CardId");

                    b.Property<int>("CardStateId");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<long>("UserSeasonLeagueId");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("CardStateId");

                    b.HasIndex("UserSeasonLeagueId");

                    b.ToTable("UserCards");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserConnection", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("ConnectionId");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("UserId", "ConnectionId");

                    b.ToTable("UserConnections");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new { Id = 1, RoleId = 1, UserId = 1 }
                    );
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserSeasonLeague", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive");

                    b.Property<long>("LeagueId");

                    b.Property<int>("Points");

                    b.Property<long>("SeasonId");

                    b.Property<DateTime>("SubscriptionDateTime");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.HasIndex("SeasonId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSeasonLeagues");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.CardQuestion", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.Card", "Card")
                        .WithMany("CardQuestions")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FaisalLeague.Data.Model.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.Choice", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.Question", "Question")
                        .WithMany("Choices")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.Expense", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.League", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.League", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.Question", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.Category", "Category")
                        .WithMany("Questions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.User", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.City", "City")
                        .WithMany("Users")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserAnswer", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.Choice", "Choice")
                        .WithMany()
                        .HasForeignKey("ChoiceId");

                    b.HasOne("FaisalLeague.Data.Model.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FaisalLeague.Data.Model.UserCard", "UserCard")
                        .WithMany("UserAnswers")
                        .HasForeignKey("UserCardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserCard", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FaisalLeague.Data.Model.CardState", "CardState")
                        .WithMany()
                        .HasForeignKey("CardStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FaisalLeague.Data.Model.UserSeasonLeague", "UserSeasonLeague")
                        .WithMany()
                        .HasForeignKey("UserSeasonLeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserConnection", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserRole", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FaisalLeague.Data.Model.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FaisalLeague.Data.Model.UserSeasonLeague", b =>
                {
                    b.HasOne("FaisalLeague.Data.Model.League", "League")
                        .WithMany()
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FaisalLeague.Data.Model.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FaisalLeague.Data.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
