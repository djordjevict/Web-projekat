using Microsoft.EntityFrameworkCore;
using ProjekatStudent.DbContexts;
using ProjekatStudent.Entities;
using SQLitePCL;

namespace ProjekatStudent.Repos
{
    public class JoinRepository : IJoinRepository
    {
        private readonly StudentContext _context;

        public JoinRepository(StudentContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistJoinAsync(int examId, int studentId)
        {
            return await _context.Joins.AnyAsync(e => e.ExamId == examId && e.StudentId == studentId);
        }

        public async Task<Join> AddJoinAsync(Join join)
        {
            _context.Joins.Add(join);
            await SaveAsync();
            return join;
        }
        public async Task<Join?> GetJoinAsync(int studentId, int examId)
        {
            return await _context.Joins.Where(k => k.StudentId == studentId && k.ExamId == examId).FirstOrDefaultAsync();
        }
        public async Task DeleteJoinAsync(Join join)
        {
            _context.Joins.Remove(join);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
