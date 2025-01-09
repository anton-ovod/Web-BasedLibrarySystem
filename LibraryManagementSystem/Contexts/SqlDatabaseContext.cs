
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Contexts
{
    public class SqlDatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public SqlDatabaseContext(DbContextOptions<SqlDatabaseContext> options) : base(options) { }
    }
}
