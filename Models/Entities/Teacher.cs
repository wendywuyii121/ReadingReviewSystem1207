using System.ComponentModel.DataAnnotations;

namespace ReadingReviewSystem1207.Models.Entities {
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        public required string Name { get; set; }  
        public List<SchoolClass> Classes { get; set; } = new();  // 預設空列表
    }

}
