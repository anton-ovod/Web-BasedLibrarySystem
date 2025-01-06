using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Interfaces;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await _userRepository.GetByEmailAsync(User.Identity!.Name!);

            var model = new UserProfileViewModel
            {
                Id = currentUser!.Id,
                Name = currentUser.Name,
                Surname = currentUser.Surname,
                Email = currentUser.Email,
                Phone = currentUser.Phone,
                Age = currentUser.Age
            };

            return View(model);
        }
    }
}
