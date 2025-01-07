using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;

        public HomeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userRepository.GetByEmailAsync(User.Identity!.Name!);

            ViewData["UserInitials"] = $"{currentUser!.Name[0]}{currentUser.Surname[0]}";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await _userRepository.GetByEmailAsync(User.Identity!.Name!);

            var model = new UserProfileViewModel(currentUser!);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await _userRepository.GetByIdAsync(model.Id);

            var updatedUser = new User(model, currentUser!);

            var res = await _userRepository.UpdateAsync(updatedUser);

            if(res is null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while updating. Please try again later.", "Update Failed", ToastType.Error);
                return View(model);
            }

            ToastMessageHelper.SetToastMessage(TempData, "Profile updated successfully.", "Success", ToastType.Success);
            return RedirectToAction("Index", "Home");
        }
    }
}
