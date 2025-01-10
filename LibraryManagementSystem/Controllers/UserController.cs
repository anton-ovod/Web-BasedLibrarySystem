using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Implementations;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class UserController(IUserRepository userRepository) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await userRepository.GetByEmailAsync(User.Identity!.Name!);

            var model = new UserProfileViewModel(currentUser!);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await userRepository.GetByIdAsync(model.Id);

            var updatedUser = new User(model, currentUser!);

            var res = await userRepository.UpdateAsync(updatedUser);

            if (res is null)
            {
                ToastMessageHelper.SetToastMessage(TempData, "An error occurred while updating. Please try again later.", "Update Failed", ToastType.Error);
                return View(model);
            }

            ToastMessageHelper.SetToastMessage(TempData, "Profile updated successfully.", "Success", ToastType.Success);
            return RedirectToAction("Index", "Home");
        }
    }
}
