using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class LibraryController(IUserRepository userRepository) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await userRepository.GetByEmailAsync(User.Identity!.Name!);

            ViewData["UserInitials"] = $"{currentUser!.Name[0]}{currentUser.Surname[0]}";

            return View();
        }
    }
}
