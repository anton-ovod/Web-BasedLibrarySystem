using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("CookieAuthentication");

            TempData["ToastMessage"] = "You have been logged out.";
            TempData["ToastTitle"] = "Success";
            TempData["ToastType"] = "success";

            return RedirectToAction("LogIn", "Auth");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Please correct the errors in the form";
                TempData["ToastTitle"] = "Log In Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            // Check user credentials
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                TempData["ToastMessage"] = "Invalid email or password";
                TempData["ToastTitle"] = "Log In Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            var passwordHasher = new PasswordHasher<User>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                TempData["ToastMessage"] = "Invalid email or password";
                TempData["ToastTitle"] = "Log In Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            // Create authentication claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
            };

            // Create authentication cookie
            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null
            };

            await HttpContext.SignInAsync("CookieAuthentication", new ClaimsPrincipal(claimsIdentity), authProperties);

            TempData["ToastMessage"] = "Log In successful!";
            TempData["ToastTitle"] = "Success";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Please correct the errors in the form.";
                TempData["ToastTitle"] = "Registration Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            // Check if a user with the same email already exists
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                TempData["ToastMessage"] = "A user with this email already exists.";
                TempData["ToastTitle"] = "Registration Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            if (_context.Users.Any(u => u.Phone == model.Phone))
            {
                TempData["ToastMessage"] = "A user with this phone number already exists.";
                TempData["ToastTitle"] = "Registration Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            // Hash the password
            var passwordHasher = new PasswordHasher<User>();
            var newUser = new User(model);
            string passwordHash = passwordHasher.HashPassword(newUser, model.Password);
            newUser.PasswordHash = passwordHash;

            // Add the user to the database
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Registration successful! Please log in.";
            TempData["ToastTitle"] = "Success";
            TempData["ToastType"] = "success";

            return RedirectToAction("LogIn", "Auth");
        }
    }
}
