using ProjekatStudent.Entities;

namespace ProjekatStudent.Repos
{
    public interface IJoinRepository
    {
        Task<bool> ExistJoinAsync(int examId, int studentId);
        Task<Join> AddJoinAsync(Join join);
        Task<Join?> GetJoinAsync(int studentId, int examId);
        Task DeleteJoinAsync(Join join);
        Task SaveAsync();
    }
}
