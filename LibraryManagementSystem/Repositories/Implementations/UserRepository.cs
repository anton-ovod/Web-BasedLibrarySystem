using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class UserRepository(SqlDatabaseContext dbContext) : IUserRepository
    {
        public async Task<User?> AddAsync(User newUser)
        {
            try
            {
                var addedUser = await dbContext.Users.AddAsync(newUser);
                await dbContext.SaveChangesAsync();
                return addedUser.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<User?> DeleteAsync(int id)
        {
            try
            {
                var existingUser = await GetByIdAsync(id);
                if (existingUser is null) return null;

                await dbContext.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
                return existingUser;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Task<bool> IsPasswordCorrectAsync(User user, string providedPassword)
        {
            try
            {
                var passwordVerificationResult = User.passwordHasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
                return Task.FromResult(passwordVerificationResult == PasswordVerificationResult.Success);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.FromResult(false);
            }
        }

        public async Task<bool> IsUniqueEmailAsync(string email)
        {
            try
            {
                return !await dbContext.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> IsUniquePhoneNumberAsync(string phoneNumber)
        {
            try
            {
                return !await dbContext.Users.AnyAsync(u => u.Phone == phoneNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<User?> UpdateAsync(User updatedUser)
        {
            try
            {
                var existingUser = await GetByIdAsync(updatedUser.Id);
                if (existingUser is null) return null;

                await dbContext.Users
                    .Where(u => u.Id == updatedUser.Id)
                    .ExecuteUpdateAsync(u => u
                       .SetProperty(u => u.Name, updatedUser.Name)
                       .SetProperty(u => u.Surname, updatedUser.Surname)
                       .SetProperty(u => u.Email, updatedUser.Email)
                       .SetProperty(u => u.Phone, updatedUser.Phone)
                       .SetProperty(u => u.Age, updatedUser.Age)
                       .SetProperty(u => u.PasswordHash, updatedUser.PasswordHash));

                await dbContext.SaveChangesAsync();
                return updatedUser;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
