using Microsoft.EntityFrameworkCore;
using ProjekatStudent.DbContexts;
using ProjekatStudent.Entities;

namespace ProjekatStudent.Repos
{
    public class ExamRepository : IExamRepository
    {
        private readonly StudentContext _context;

        public ExamRepository(StudentContext dbContext)
        {
            _context = dbContext;
        }       

        public async Task<Exam?> GetExamByIdAsync(int id)
        {
            return await _context.Exams.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Exam?> GetExamByNameAsync(string name)
        {
            return await _context.Exams.Where(e => e.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsAsync()
        {
            return await _context.Exams.OrderBy(e => e.Name).ToListAsync();
        }
        public async Task<bool> ExistExamByIdAsync(int id)
        {
            return await _context.Exams.AnyAsync(e => e.Id == id);
        }
        public async Task<bool> ExistExamByNameAsync(string name)
        {
            return await _context.Exams.AnyAsync(e => e.Name == name);
        }
        public async Task<Exam> AddExamAsync(Exam exam)
        {
            _context.Exams.Add(exam);
            await SaveAsync();
            return exam;
        }

        public async Task DeleteExamAsync(Exam exam)
        {
            _context.Exams.Remove(exam);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
