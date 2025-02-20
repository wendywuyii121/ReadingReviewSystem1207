using System.ComponentModel.DataAnnotations;

namespace ReadingReviewSystem1207.Models.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        public required string Name { get; set; }  
        public int ClassId { get; set; }
        public required SchoolClass Class { get; set; } 
    }

}