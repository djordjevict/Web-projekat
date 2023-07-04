using ProjekatStudent.Entities;

namespace ProjekatStudent.Repos
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task SaveAsync();
    }
}
