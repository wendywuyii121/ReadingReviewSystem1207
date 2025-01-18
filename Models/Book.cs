// Models/Book.cs
namespace ReadingReviewSystem1207.Models
{
    // 書籍模型
    public class Book
    {
        public int Id { get; set; }          // 主鍵，書籍的唯一識別碼
        public string Title { get; set; }    // 書名
        public string Review { get; set; }   // 書籍評論
        public string CoverImageUrl { get; set; } // 書籍封面圖片 URL
    }
}
