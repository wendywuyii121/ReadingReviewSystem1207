using System.ComponentModel.DataAnnotations;
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

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public int StudentId { get; set; }

        public Student Student { get; set; } = null!;

        public bool IsReviewed { get; set; } = false;
    }
}
