using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {

        private readonly SqlDatabaseContext _context;

        public UserRepository(SqlDatabaseContext context)
        {
            _context = context;
        }

        public async Task<User?> AddAsync(User user)
        {
            try
            {
                var addedUser = await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
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

                await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
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
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
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
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
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
                return !await _context.Users.AnyAsync(u => u.Email == email);
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
                return !await _context.Users.AnyAsync(u => u.Phone == phoneNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<User?> UpdateAsync(User user)
        {
            try
            {
                var existingUser = await GetByIdAsync(user.Id);
                if (existingUser is null) return null;

                await _context.Users
                    .Where(u => u.Id == user.Id)
                    .ExecuteUpdateAsync(u => u
                       .SetProperty(u => u.Name, user.Name)
                       .SetProperty(u => u.Surname, user.Surname)
                       .SetProperty(u => u.Email, user.Email)
                       .SetProperty(u => u.Phone, user.Phone)
                       .SetProperty(u => u.Age, user.Age)
                       .SetProperty(u => u.PasswordHash, user.PasswordHash));

                await _context.SaveChangesAsync();
                return user;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
