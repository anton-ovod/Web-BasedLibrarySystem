using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult LogIn()
        {
            return View();
        }
    }
}
