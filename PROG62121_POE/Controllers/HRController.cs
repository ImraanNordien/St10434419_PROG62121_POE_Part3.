using Microsoft.AspNetCore.Mvc;
using PROG62121_POE.Data;
using PROG62121_POE.Models;

namespace PROG62121_POE.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAuthorized()
        {
            return HttpContext.Session.GetString("Role") == "HR";
        }

        public IActionResult Index()
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult AddUser()
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(user);

            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditUser(int id)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User updatedUser)
        {
            if (!IsAuthorized())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(updatedUser.Id);
            if (user == null) return NotFound();

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            user.Role = updatedUser.Role;
            user.HourlyRate = updatedUser.Role == "Lecturer" ? updatedUser.HourlyRate : 0;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
