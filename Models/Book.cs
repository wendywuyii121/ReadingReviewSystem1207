using System;
using System.ComponentModel.DataAnnotations;

namespace ReadingReviewSystem1207.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        public string? OwnerId { get; set; } // 用戶 ID
        public ApplicationUser? Owner { get; set; } // 關聯用戶

        public string? CoverImagePath { get; set; } // ✅ 確保這個屬性存在

        public string? Review { get; set; } // ✅ 新增心得屬性

    }
}
