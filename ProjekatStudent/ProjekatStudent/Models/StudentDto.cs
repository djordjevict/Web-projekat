using ProjekatStudent.Entities;

namespace ProjekatStudent.Models
{
    public class StudentDto
    {
        public int Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfPassedExams
        {
            get
            {
                return PassedExams.Count;
            }
        }
        public ICollection<Join> PassedExams { get; set; }
               = new List<Join>();

    }
}
