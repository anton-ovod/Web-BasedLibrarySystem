using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class AuthController : Controller
    {
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
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Please correct the errors in the form.";
                TempData["ToastTitle"] = "Registration Failed";
                TempData["ToastType"] = "error";
                return View(model);
            }

            TempData["ToastMessage"] = "Registration successful! Please log in.";
            TempData["ToastTitle"] = "Success";
            TempData["ToastType"] = "success";

            return RedirectToAction("LogIn", "Auth");
        }
    }
}
