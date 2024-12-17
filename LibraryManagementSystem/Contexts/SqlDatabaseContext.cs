using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Contexts
{
    public class SqlDatabaseContext : DbContext
    {
        public SqlDatabaseContext(DbContextOptions<SqlDatabaseContext> options) : base(options) { }
    }
}
