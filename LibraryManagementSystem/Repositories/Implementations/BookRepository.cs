using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Repositories.Interfaces;

namespace LibraryManagementSystem.Repositories.Implementations
{
    public class BookRepository: IBookRepository
    {
        private readonly SqlDatabaseContext _dbContext;

        public BookRepository(SqlDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        
    }
}
