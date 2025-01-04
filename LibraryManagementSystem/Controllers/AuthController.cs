using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class AuthController : Controller
    {

        private readonly SqlDatabaseContext _context;

        public AuthController(SqlDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Please correct the errors in the form.";
                TempData["ToastTitle"] = "Registration Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            // Create a new user entity from the model
            var user = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Phone = model.Phone,
                Age = model.Age,
                Gender = model.Gender
            };

            // Add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Registration successful! Please log in.";
            TempData["ToastTitle"] = "Success";
            TempData["ToastType"] = "success";

            return RedirectToAction("LogIn", "Auth");
        }
    }
}
