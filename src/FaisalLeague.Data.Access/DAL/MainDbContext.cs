using System;
using System.IO;
using System.Reflection;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FaisalLeague.Data.Access.Helpers;

namespace FaisalLeague.Data.Access.DAL
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var mappings = MappingsHelper.GetMainMappings();

            foreach (var mapping in mappings)
            {
                mapping.Visit(modelBuilder);
            }

            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder builder)
        {
            // Seed Cities
            builder.Entity<City>().HasData(new City { Id = 1, Name = "اللاذقية", Sort = 1 },
                new City { Id = 2, Name = "دمشق", Sort = 2 },
                new City { Id = 3, Name = "حمص", Sort = 3 },
                new City { Id = 4, Name = "حلب", Sort = 4 },
                new City { Id = 5, Name = "حماه", Sort = 5 }
                );

            // Seed Admin
            builder.Entity<User>().HasData(new User { Id = 1, Username = "admin", FirstName = "مدير النظام", LastName = "مدير النظام", IsDeleted = false, Password = "$2a$11$Y5buKCEzLVmY7XpKdBCoI.dGPlXkZg15tDcbKZTuPRVmt40Yx36Sm", CityId=1, DOB=new DateTime(1985,05,27), Image="defaultUser.jpg", Email="admin@admin.com", MiddleName="مدير النظام", Mobile="0933238330" });

            // Seed Roles
            builder.Entity<Role>().HasData(new Role { Id = 1,Name="Administrator", Description = "مدير النظام" });

            // Set Admin Role
            builder.Entity<UserRole>().HasData(new UserRole { Id = 1, RoleId = 1, UserId = 1 });

            builder.Entity<League>().HasData(new League { Id = 1, Name = "الدوريات", ParentId = null, Sort = 1 });
            builder.Entity<League>().HasData(new League { Id = 2, Name = "الاسباني", ParentId = 1, Sort = 1 });
            builder.Entity<League>().HasData(new League { Id = 3, Name = "الايطالي", ParentId = 1, Sort = 2 });
            builder.Entity<League>().HasData(new League { Id = 4, Name = "الانجليزي", ParentId = 1, Sort = 3 });
            builder.Entity<League>().HasData(new League { Id = 5, Name = "الألماني", ParentId = 1, Sort = 4 });

            builder.Entity<League>().HasData(new League { Id = 6, Name = "الفرق", ParentId = null, Sort = 2 });
            builder.Entity<League>().HasData(new League { Id = 7, Name = "مانشستر يونايتد", ParentId = 6, Sort = 1 });
            builder.Entity<League>().HasData(new League { Id = 8, Name = "مانشستر سيتي", ParentId = 6, Sort = 2 });
            builder.Entity<League>().HasData(new League { Id = 9, Name = "ريال مدريد", ParentId = 6, Sort = 3 });
            builder.Entity<League>().HasData(new League { Id = 10, Name = "برشلونة", ParentId = 6, Sort = 4 });
            builder.Entity<League>().HasData(new League { Id = 11, Name = "أتلتيكو مدريد", ParentId = 6, Sort = 5 });
            builder.Entity<League>().HasData(new League { Id = 12, Name = "تشيلسي", ParentId = 6, Sort = 6 });
            builder.Entity<League>().HasData(new League { Id = 13, Name = "أرسنال", ParentId = 6, Sort = 7 });
            builder.Entity<League>().HasData(new League { Id = 14, Name = "بايرن ميونخ", ParentId = 6, Sort = 8 });
            builder.Entity<League>().HasData(new League { Id = 15, Name = "يوفينتوس", ParentId = 6, Sort = 9 });
            builder.Entity<League>().HasData(new League { Id = 16, Name = "روما", ParentId = 6, Sort = 10 });

            builder.Entity<League>().HasData(new League { Id = 17, Name = "المنتخبات", ParentId = null, Sort = 3 });
            builder.Entity<League>().HasData(new League { Id = 18, Name = "انجلترا", ParentId = 17, Sort = 1 });
            builder.Entity<League>().HasData(new League { Id = 19, Name = "فرنسا", ParentId = 17, Sort = 1 });
            builder.Entity<League>().HasData(new League { Id = 20, Name = "البرازيل", ParentId = 17, Sort = 1 });
        }
    }
}