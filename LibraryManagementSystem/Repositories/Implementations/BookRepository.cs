using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MimeKit.Tnef;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class BookRepository(SqlDatabaseContext dbContext) : IBookRepository
    {
        public async Task<Book?> AddAsync(Book newBook)
        {
            try
            {
                var addedBook = await dbContext.Books.AddAsync(newBook);
                await dbContext.SaveChangesAsync();
                return addedBook.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Book?> DeleteAsync(int id)
        {
            try
            {
                var existingBook = await GetByIdAsync(id);
                if (existingBook is null) return null;

                await dbContext.Books.Where(b => b.Id == id).ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
                return existingBook;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Book>> GetByTitleAsync(string title, int userId, int page, int pageSize)
        {
            return await dbContext.Books
                .Where(b => b.Title.Contains(title) && b.UserId == userId)
                .OrderByDescending(b => b.LastUpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByUserId(int userId, int page, int pageSize)
        {
            return await dbContext.Books
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.LastUpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalBooksByTitleAsync(string title, int userId)
        {
            return await dbContext.Books
                .Where(b => b.Title.Contains(title) && b.UserId == userId)
                .CountAsync();
        }

        public async Task<int> GetTotalBooksByUserId(int userId)
        {
            return await dbContext.Books
                .Where(b => b.UserId == userId)
                .CountAsync();
        }

        public async Task<bool> IsUniqueTitleAsync(string title, int? bookId, int userId)
        {
            try
            {
                return !await dbContext.Books.Where(b => b.Id != bookId && b.UserId == userId).AnyAsync(b => b.Title == title);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Book?> UpdateAsync(Book updatedBook)
        {
            try
            {
                var existingBook = await GetByIdAsync(updatedBook.Id);
                if (existingBook is null) return null;

                await dbContext.Books
                    .Where(b => b.Id == updatedBook.Id)
                    .ExecuteUpdateAsync(u => u
                        .SetProperty(b => b.Title, updatedBook.Title)
                        .SetProperty(b => b.Author, updatedBook.Author) 
                        .SetProperty(b => b.Description, updatedBook.Description)
                        .SetProperty(b => b.Genres, updatedBook.Genres)
                        .SetProperty(b => b.PagesNumber, updatedBook.PagesNumber)
                        .SetProperty(b => b.ReadPagesNumber, updatedBook.ReadPagesNumber)
                        .SetProperty(b => b.Cover, updatedBook.Cover));

                await dbContext.SaveChangesAsync();
                return updatedBook;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


    }
}
