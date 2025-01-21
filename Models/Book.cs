using System.ComponentModel.DataAnnotations;

namespace ReadingReviewSystem1207.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "書名是必填項")]
        public string Title { get; set; }

        [Required(ErrorMessage = "心得是必填項")]
        public string Review { get; set; }

        // 設置 CoverImageUrl 可為 null
        public string? CoverImageUrl { get; set; }
    }
}
