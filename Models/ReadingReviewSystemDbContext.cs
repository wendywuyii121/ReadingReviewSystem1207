// Models/ReadingReviewSystemDbContext.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ReadingReviewSystem1207.Models
{
    // 資料庫上下文，負責與資料庫的互動
    public class ReadingReviewSystemDbContext : DbContext
    {
        // 建構子，用來注入資料庫配置
        public ReadingReviewSystemDbContext(DbContextOptions<ReadingReviewSystemDbContext> options)
            : base(options) { }

        // 定義與資料庫中的表格對應的集合
        public DbSet<Book> Books { get; set; }
    }
}
