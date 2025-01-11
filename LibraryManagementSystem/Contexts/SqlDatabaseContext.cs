
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Contexts
{
    public class SqlDatabaseContext(DbContextOptions<SqlDatabaseContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

    }
}
