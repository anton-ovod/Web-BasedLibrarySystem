using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<bool> IsUniqueEmailAsync(string email);
        Task<bool> IsUniquePhoneNumberAsync(string phoneNumber);
        Task <bool> IsPasswordCorrectAsync(User user, string providedPassword);
        Task<User?> AddAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<User?> DeleteAsync(int id);
    }
}
