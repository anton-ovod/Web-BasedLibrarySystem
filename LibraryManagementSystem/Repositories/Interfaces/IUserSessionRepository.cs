using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories.Interfaces
{
    public interface IUserSessionRepository
    {
        Task<UserSession?> AddAsync(UserSession session);
        Task<IEnumerable<UserSession>> GetAllAsync(int userId, int page, int pageSize);
        Task<UserSession?> DeactivateAsync(string sessionId); // delete from database
        Task<UserSession?> UpdateAsync(UserSession updatedUserSession);
        Task<UserSession?> GetByIdAsync(string sessionId);
    }
}
