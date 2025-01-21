using System.ComponentModel.DataAnnotations;

namespace ReadingReviewSystem1207.Models
{
    // 書籍模型
    public class Book
    {
        public int Id { get; set; }          // 主鍵，書籍的唯一識別碼
        [Required]
        public string Title { get; set; } = string.Empty;    // 書名
        [Required]
        public string Review { get; set; } = string.Empty;   // 書籍評論
        public string? CoverImageUrl { get; set; } // 書籍封面圖片 URL，設為可選
    }
}
