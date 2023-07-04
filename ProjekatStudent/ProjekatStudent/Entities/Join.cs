using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjekatStudent.Entities
{
    public class Join
    {

        [Required]
        [Range(6,10)]
        public int Grade { get; set; }

        [Required]
        public string ExaminationPeriod { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
        public int StudentId { get; set; }

        [ForeignKey("ExamId")]
        public Exam? Exam { get; set; }
        public int ExamId { get; set; }
    }
}
