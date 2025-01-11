using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class BookController(IBookRepository bookRepository) : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ToastMessageHelper.SetToastMessage(TempData, "Please correct the errors in the form", "Addition fails", ToastType.Error);
                return View(model);
            }

            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(nameIdentifier))
            {
                throw new InvalidOperationException("NameIdentifier claim not found.");
            }

            if (!await bookRepository.IsUniqueTitleAsync(model.Title, null, int.Parse(nameIdentifier)))
            {
                ToastMessageHelper.SetToastMessage(TempData, "Title with this book is already in your library", "Update Failed", ToastType.Error);
                return View(model);
            }

            var newBook = new Book(model, int.Parse(nameIdentifier));

            var result = await bookRepository.AddAsync(newBook);

            if (result is null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while adding new book. Please try again later.", "Addition Failed", ToastType.Error);
                return View(model);
            }

            ToastMessageHelper.SetToastMessage(TempData, "Addition successful!", "Success", ToastType.Success);

            return RedirectToAction("Index", "Library");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await bookRepository.GetByIdAsync(id);
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (book == null)
            {
                return NotFound();
            }

            if (book.UserId.ToString() != currentUserId)
            {
                return RedirectToAction("Index", "Library");
            }

            var model = new UpdateBookViewModel(book);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(UpdateBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingBook = await bookRepository.GetByIdAsync(model.Id);

            if (existingBook == null)
            {
                return NotFound();
            }

            if (!await bookRepository.IsUniqueTitleAsync(model.Title, existingBook.Id, existingBook.UserId))
            {
                ToastMessageHelper.SetToastMessage(TempData, "Title with this book is already in your library", "Update Failed", ToastType.Error);
                return View(model);
            }

            var updatedBook = new Book(model, existingBook);

            var result = await bookRepository.UpdateAsync(updatedBook);

            if (result is null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while updating the book. Please try again later.", "Update Failed", ToastType.Error);
                return View(model);
            }

            ToastMessageHelper.SetToastMessage(TempData, "Update successful!", "Success", ToastType.Success);
            return RedirectToAction("Index", "Library");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int bookId)
        {
            var book = await bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "The book you are trying to delete does not exist.", "Deletion Failed", ToastType.Error);
                return RedirectToAction("Index", "Library");
            }

            var result = await bookRepository.DeleteAsync(bookId);

            if (result is null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while deleting the book. Please try again.", "Deletion Failed", ToastType.Error);
                return RedirectToAction("Index", "Library");
            }

            ToastMessageHelper.SetToastMessage(TempData, "Book deleted successfully.", "Success", ToastType.Success);

            return RedirectToAction("Index", "Library");
        }
    }
}
