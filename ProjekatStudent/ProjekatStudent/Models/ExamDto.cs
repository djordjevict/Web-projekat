using ProjekatStudent.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjekatStudent.Models
{
    public class ExamDto
    {
        public string Name { get; set; }
        public int Semester { get; set; }
        public int NumberOfPassedExams
        {
            get
            {
                return ListOfPassedExams.Count;
            }
        }
        public ICollection<Join> ListOfPassedExams { get; set; }
               = new List<Join>();
    }
}
