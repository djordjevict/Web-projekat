using ProjekatStudent.Entities;

namespace ProjekatStudent.Repos
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<IEnumerable<Student?>> GetStudentsByNameAndPeriodAsync(string name, string period);
        Task<Student?> GetStudentByIndexAsync(int index);
        Task<Student?> GetStudentByIndexWithPassedExamAsync(int index);
        Task<Student?> GetStudentByIdAsync(int id);
        Task<bool> ExistStudentByIdAsync(int id);
        Task<bool> ExistStudentByIndexAsync(int index);
        Task<Student> AddStudentAsync(Student student);
        Task DeleteStudentAsync(Student student);
        Task SaveAsync();
    }
}
