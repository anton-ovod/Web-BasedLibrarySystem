using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Core.Types;

namespace LibraryManagementSystem.Controllers
{
    public class AuthController(IUserRepository userRepository, IUserSessionRepository userSessionRepository, EmailService emailService) : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Library");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while logging out. Please try again later.", "Log Out Failed", ToastType.Error);
                return RedirectToAction("LogIn", "Auth");
            }
            
            ToastMessageHelper.SetToastMessage(TempData, "You have been logged out.", "Success", ToastType.Success);

            return RedirectToAction("LogIn", "Auth");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ToastMessageHelper.SetToastMessage(TempData, "Please correct the errors in the form", "Log In Failed", ToastType.Error);

                return View(model);
            }

            var user = await userRepository.GetByEmailAsync(model.Email);
            if (user == null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "Invalid email or password", "Log In Failed", ToastType.Error);
                
                return View(model);
            }

            if (!await userRepository.IsPasswordCorrectAsync(user, model.Password))
            {
                ToastMessageHelper.SetToastMessage(TempData, "Invalid email or password", "Log In Failed", ToastType.Error);
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? DateTime.UtcNow.AddDays(1) : null
            };

            try
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }
            catch
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while logging in. Please try again later.", "Log In Failed", ToastType.Error);
                return View(model);
            }

            ToastMessageHelper.SetToastMessage(TempData, "Log In successful!", "Success", ToastType.Success);
            
            return RedirectToAction("Index", "Library");

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
                ToastMessageHelper.SetToastMessage(TempData, "Please correct the errors in the form", "Registration Failed", ToastType.Error);
                return View(model);
            }

            if (!await userRepository.IsUniqueEmailAsync(model.Email))
            {
                ToastMessageHelper.SetToastMessage(TempData, "Provided data is not correct", "Registration Failed", ToastType.Error);
                return View(model);
            }

            if (!await userRepository.IsUniquePhoneNumberAsync(model.Phone))
            {
                ToastMessageHelper.SetToastMessage(TempData, "Provided data is not correct", "Registration Failed", ToastType.Error);
                return View(model);
            }

            var newUser = new User(model);

            var result = await userRepository.AddAsync(newUser);

            if(result is null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while registering. Please try again later.", "Registration Failed", ToastType.Error);
                return View(model);
            }

            ToastMessageHelper.SetToastMessage(TempData, "Registration successful!", "Success", ToastType.Success);
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
                ToastMessageHelper.SetToastMessage(TempData, "Please correct the errors in the form", "Reset Password Failed", ToastType.Error);
                return View(model);
            }

            var user = await userRepository.GetByEmailAsync(model.Email);
            if (user == null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "Provided data is not correct", "Reset Password Failed", ToastType.Error);
                return RedirectToAction("LogIn", "Auth");
            }

            var newPassword = PasswordGenerator.GeneratePassword();
            user.PasswordHash = Models.User.passwordHasher.HashPassword(user, newPassword);

            var result = await userRepository.UpdateAsync(user);

            if (result is null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while resetting the password. Please try again later.", "Reset Password Failed", ToastType.Error);
                return View(model);
            }
            try
            {
                var emailBody = $"<p>Hello {user.Name} {user.Surname},</p>" +
                                $"<p>Your password has been reset. Here is your new temporary password:</p>" +
                                $"<p><strong>{newPassword}</strong></p>" +
                                $"<p>Please log in and change your password immediately.</p>";

                await emailService.SendEmailAsync(user.Email, "Password Reset Notification", emailBody);
            }
            catch
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while sending the email. Please try again later.", "Reset Password Failed", ToastType.Error);
                return RedirectToAction("LogIn", "Auth");
            }
            
            ToastMessageHelper.SetToastMessage(TempData, "Password reset successful! Check your email for the new password.", "Success", ToastType.Success);

            return RedirectToAction("LogIn", "Auth");
        }

    }
}
