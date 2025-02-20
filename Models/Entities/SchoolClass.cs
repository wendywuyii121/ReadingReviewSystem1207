using System.ComponentModel.DataAnnotations;

namespace ReadingReviewSystem1207.Models.Entities
{
    public class SchoolClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ClassName { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public int TeacherId { get; set; }

        public Teacher Teacher { get; set; } = null!;

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
