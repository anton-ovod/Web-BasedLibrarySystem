using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.ViewModels
{
    public class LibraryViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string? SearchQuery { get; set; }
    }
}
