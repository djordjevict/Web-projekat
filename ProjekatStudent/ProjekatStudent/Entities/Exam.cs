using System.ComponentModel.DataAnnotations;

namespace ProjekatStudent.Entities
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Range(1,8)]
        public int Semester { get; set; }

        public ICollection<Join> ListOfPassedExams { get; set; }
               = new List<Join>();
    }
}
