using Microsoft.EntityFrameworkCore;
using ReadingReviewSystem1207.Models.Entities;

namespace ReadingReviewSystem1207.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
