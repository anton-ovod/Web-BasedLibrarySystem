using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book model)
        {
            return RedirectToAction("Index", "Library");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var book = new Book
            {
                Id = id,
                Title = "Example Book Title",
                Author = "John Doe",
                Description = "This is an example book description. It gives details about the book.",
                Genres = "Fiction, Adventure",
                Cover = null,
                PagesNumber = 350,
                ReadPagesNumber = 120,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            return View(book);
        }

        [HttpPost]
        public IActionResult Update(Book model, IFormFile? newCover)
        {
            return RedirectToAction("Index", "Library");
        }
    }
}
