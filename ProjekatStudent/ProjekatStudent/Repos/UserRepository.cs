using Microsoft.EntityFrameworkCore;
using ProjekatStudent.DbContexts;
using ProjekatStudent.Entities;

namespace ProjekatStudent.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly StudentContext _context;

        public UserRepository(StudentContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Where(s => s.Username == username).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await SaveAsync();
            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
