using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ReadingReviewSystem1207.Models.Entities;

namespace ReadingReviewSystem1207.Models.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string StudentId { get; set; } // 學生 ID
        [ForeignKey("StudentId")]
        public ApplicationUser Student { get; set; } // 關聯學生

        [Required]
        public string TeacherId { get; set; } // 教師 ID
        [ForeignKey("TeacherId")]
        public ApplicationUser Teacher { get; set; } // 關聯教師

        public bool IsReviewed { get; set; } = false;
    }
}
