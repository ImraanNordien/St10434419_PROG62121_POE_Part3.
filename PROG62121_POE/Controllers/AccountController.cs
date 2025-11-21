using Microsoft.AspNetCore.Mvc;
using PROG62121_POE.Data;
using PROG62121_POE.Models;
using System.Linq;

namespace PROG62121_POE.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login() => View();

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // ✅ Store all relevant session info
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("UserFullName", $"{user.FirstName} {user.LastName}");
                HttpContext.Session.SetString("Email", user.Email);

                // ✅ Store hourly rate if lecturer
                if (user.Role == "Lecturer")
                    HttpContext.Session.SetString("HourlyRate", user.HourlyRate.ToString());

                // Redirect by role
                return user.Role switch
                {
                    "Lecturer" => RedirectToAction("Index", "Lecturer"),
                    "ProgrammeCoordinator" => RedirectToAction("Index", "ProgrammeCoordinator"),
                    "AcademicManager" => RedirectToAction("Index", "AcademicManager"),
                    "HR" => RedirectToAction("Index", "HR"),
                    _ => RedirectToAction("Login")
                };
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
