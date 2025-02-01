using System.ComponentModel.DataAnnotations;

namespace ReadingReviewSystem1207.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Review { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; } // 允許 CoverImageUrl 為 null
    }
}
