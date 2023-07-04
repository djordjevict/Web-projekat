using ProjekatStudent.Entities;

namespace ProjekatStudent.Repos
{
    public interface IExamRepository
    {
        Task<IEnumerable<Exam>> GetExamsAsync();
        Task<Exam?> GetExamByIdAsync(int id);
        Task<Exam?> GetExamByNameAsync(string name);
        Task<bool> ExistExamByIdAsync(int id);
        Task<bool> ExistExamByNameAsync(string name);
        Task<Exam> AddExamAsync(Exam exam);
        Task DeleteExamAsync(Exam exam);
        Task SaveAsync();
    }
}
