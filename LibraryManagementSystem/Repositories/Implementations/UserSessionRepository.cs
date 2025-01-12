using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class UserSessionRepository(SqlDatabaseContext dbContext) : IUserSessionRepository
    {
        public async Task<UserSession?> AddAsync(UserSession session)
        {
            try
            {
                await dbContext.UserSessions.AddAsync(session);
                await dbContext.SaveChangesAsync();
                return session;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserSession?> DeactivateAsync(string sessionId)
        {
            try
            {
                var existingSession = await GetByIdAsync(sessionId);
                if (existingSession is null) return null;

                await dbContext.UserSessions.Where(s => s.SessionId == sessionId).ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
                return existingSession;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<UserSession>> GetAllAsync(int userId, int page, int pageSize)
        {
            return await dbContext.UserSessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<UserSession?> GetByIdAsync(string sessionId)
        {
            try
            {
                return await dbContext.UserSessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserSession?> UpdateAsync(UserSession updatedUserSession)
        {
            try
            {
                var existingSession = await GetByIdAsync(updatedUserSession.SessionId);
                if (existingSession is null) return null;

                await dbContext.UserSessions
                    .Where(s => s.SessionId == updatedUserSession.SessionId)
                    .ExecuteUpdateAsync(u => u
                        .SetProperty(s => s.SessionId, updatedUserSession.SessionId)
                        .SetProperty(s => s.UserId, updatedUserSession.UserId)
                        .SetProperty(s => s.Device, updatedUserSession.Device)
                        .SetProperty(s => s.IpAddress, updatedUserSession.IpAddress)
                        .SetProperty(s => s.CreatedAt, updatedUserSession.CreatedAt)
                        .SetProperty(s => s.ExpiresAt, updatedUserSession.ExpiresAt)
                        );

                await dbContext.SaveChangesAsync();
                return updatedUserSession;
            }
            catch
            {
                return null;
            }
        }
    }
}
