using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> AddAsync(Book newBook);
        Task<Book?> UpdateAsync(Book updatedBook);
        Task<Book?> DeleteAsync(int id);
        Task<IEnumerable<Book>> GetByUserId(int userId, int page, int pageSize);
        Task<int> GetTotalBooksByUserId(int userId);
        Task<IEnumerable<Book>> GetByTitleAsync(string title, int userId, int page, int pageSize);
        Task<int> GetTotalBooksByTitleAsync(string title, int userId);
        Task<bool> IsUniqueTitleAsync(string title, int? bookId, int userId);
    }
}
