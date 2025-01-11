using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LibraryManagementSystem.Models;
using Microsoft.Security.Application;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class LibraryController(IUserRepository userRepository, IBookRepository bookRepository) : Controller
    {
        private readonly int _pageSize = 4;

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(nameIdentifier))
            {
                throw new InvalidOperationException("NameIdentifier claim not found.");
            }

            var currentUser = await userRepository.GetByIdAsync(int.Parse(nameIdentifier));

            if (currentUser == null)
            {
                throw new InvalidOperationException($"User with ID {nameIdentifier} not found.");
            }

            ViewData["UserInitials"] = $"{currentUser.Name[0]}{currentUser.Surname[0]}";

            var books = await bookRepository.GetByUserId(currentUser.Id, page, _pageSize);
            var totalBooks = await bookRepository.GetTotalBooksByUserId(currentUser.Id);

            int totalPages = (int)Math.Ceiling((double)totalBooks / _pageSize);

            var model = new LibraryViewModel
            {
                Books = books,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchQuery = null
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string searchQuery, int page = 1)
        {
            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(nameIdentifier))
            {
                throw new InvalidOperationException("NameIdentifier claim not found.");
            }

            var currentUser = await userRepository.GetByIdAsync(int.Parse(nameIdentifier));

            if (currentUser == null)
            {
                throw new InvalidOperationException($"User with ID {nameIdentifier} not found.");
            }

            ViewData["UserInitials"] = $"{currentUser.Name[0]}{currentUser.Surname[0]}";

            IEnumerable<Book> books;
            int totalBooks;
            searchQuery = Sanitizer.GetSafeHtmlFragment(searchQuery);

            if (string.IsNullOrEmpty(searchQuery))
            {
                books = await bookRepository.GetByUserId(currentUser.Id, page, _pageSize);
                totalBooks = await bookRepository.GetTotalBooksByUserId(currentUser.Id);
            }
            else
            {
                books = await bookRepository.GetByTitleAsync(searchQuery, currentUser.Id, page, _pageSize);
                totalBooks = await bookRepository.GetTotalBooksByTitleAsync(searchQuery, currentUser.Id);
            }

            int totalPages = (int)Math.Ceiling((double)totalBooks / _pageSize);

            var model = new LibraryViewModel
            {
                Books = books,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchQuery = searchQuery
            };

            return View(model);
        }
    }
}
