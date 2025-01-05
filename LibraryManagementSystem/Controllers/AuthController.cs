using LibraryManagementSystem.Contexts;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
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
        private readonly EmailService _emailService;
        public AuthController(SqlDatabaseContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Please correct the errors in the form.";
                TempData["ToastTitle"] = "Reset Password Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            // Check if the user exists
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                // Prevent revealing if an email exists in the system
                TempData["ToastMessage"] = "If the email exists in our system, a reset password link will be sent.";
                TempData["ToastTitle"] = "Reset Password";
                TempData["ToastType"] = "info";
                return RedirectToAction("LogIn", "Auth");
            }

            // Generate a new temporary password
            var newPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, newPassword);

            // Update the user's password
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error (optional)
                // _logger.LogError(ex, "Error updating password for user with email {Email}", model.Email);

                TempData["ToastMessage"] = "An error occurred while resetting your password. Please try again later.";
                TempData["ToastTitle"] = "Reset Password Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            // Send the new password to the user via email
            try
            {
                var emailBody = $"<p>Hello {user.Name} {user.Surname},</p>" +
                                $"<p>Your password has been reset. Here is your new temporary password:</p>" +
                                $"<p><strong>{newPassword}</strong></p>" +
                                $"<p>Please log in and change your password immediately.</p>";

                await _emailService.SendEmailAsync(user.Email, "Password Reset Notification", emailBody);
            }
            catch (Exception ex)
            {
                // Log error (optional)
                // _logger.LogError(ex, "Error sending password reset email to {Email}", model.Email);

                TempData["ToastMessage"] = "Password reset was successful, but we couldn't send the email. Please contact support.";
                TempData["ToastTitle"] = "Email Sending Failed";
                TempData["ToastType"] = "warning";
                return RedirectToAction("LogIn", "Auth");
            }

            // Inform the user about successful password reset
            TempData["ToastMessage"] = "If the email exists, a new password has been sent to your email.";
            TempData["ToastTitle"] = "Success";
            TempData["ToastType"] = "success";

            return RedirectToAction("LogIn", "Auth");
        }

    }
}
