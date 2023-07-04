using Microsoft.EntityFrameworkCore;
using ProjekatStudent.DbContexts;
using ProjekatStudent.Entities;

namespace ProjekatStudent.Repos
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;

        public StudentRepository(StudentContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Student?> GetStudentByIndexAsync(int index)
        {
                return await _context.Students.Where(s => s.Index == index).FirstOrDefaultAsync();
        }

        public async Task<Student?> GetStudentByIndexWithPassedExamAsync(int index)
        {
            return await _context.Students
                                .Include(s => s.PassedExams)
                                .ThenInclude(e => e.Exam)
                                .Where(s => s.Index == index)
                                .FirstOrDefaultAsync(); ;
        }

        public async Task<IEnumerable<Student?>> GetStudentsByNameAndPeriodAsync(string name, string period)
        {
            return await _context.Students
                .Include(s => s.PassedExams)
                .ThenInclude(e => e.Exam)
                .Where(a => a.PassedExams.Any(ab => ab.ExaminationPeriod == period && ab.Exam.Name == name))
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students.Include(s=> s.PassedExams).Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await _context.Students.OrderBy(s => s.Index).ToListAsync();
        }

        public async Task<bool> ExistStudentByIdAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> ExistStudentByIndexAsync(int index)
        {
            return await _context.Students.AnyAsync(s => s.Index == index);
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await SaveAsync();
            return student;
        }

        public async Task DeleteStudentAsync(Student student)
        {
            _context.Students.Remove(student);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
