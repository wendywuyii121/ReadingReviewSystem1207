using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadingReviewSystem1207.Models;
using ReadingReviewSystem1207.Models.Entities;

namespace ReadingReviewSystem1207.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> // 改用 ApplicationUser
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<ApplicationUser> AspNetUsers { get; set; } // 🔹 改用 ApplicationUser
    }
}
