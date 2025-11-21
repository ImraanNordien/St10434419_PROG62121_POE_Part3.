using Microsoft.AspNetCore.Mvc;
using PROG62121_POE.Models;
using PROG62121_POE.Services;
using PROG62121_POE.Data;
using System.IO;
using System.Threading.Tasks;

namespace PROG62121_POE.Controllers
{
    public class LecturerController : Controller
    {
        private readonly IClaimRepository _claimRepo;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public LecturerController(IClaimRepository claimRepo, IWebHostEnvironment env, ApplicationDbContext context)
        {
            _claimRepo = claimRepo;
            _env = env;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("Login", "Account");

            int? lecturerId = HttpContext.Session.GetInt32("UserId");
            if (lecturerId == null)
                return RedirectToAction("Login", "Account");

            var lecturer = _context.Users.FirstOrDefault(u => u.Id == lecturerId);
            if (lecturer == null)
                return RedirectToAction("Login", "Account");

            var claims = await _claimRepo.GetClaimsByLecturerIdAsync(lecturer.Id);
            ViewData["Claims"] = claims;
            ViewData["LecturerName"] = $"{lecturer.FirstName} {lecturer.LastName}";
            ViewData["HourlyRate"] = lecturer.HourlyRate;

            return View(new Claim());
        }

        [HttpPost]
        public async Task<IActionResult> Index(Claim claim, IFormFile uploadedFile)
        {
            if (HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("Login", "Account");

            int? lecturerId = HttpContext.Session.GetInt32("UserId");
            if (lecturerId == null)
                return RedirectToAction("Login", "Account");

            var lecturer = _context.Users.FirstOrDefault(u => u.Id == lecturerId);
            if (lecturer == null)
                return RedirectToAction("Login", "Account");

            if (claim.HoursWorked <= 0 || uploadedFile == null)
            {
                TempData["Error"] = "Please fill in all fields and upload a file.";
                var claims = await _claimRepo.GetClaimsByLecturerIdAsync(lecturer.Id);
                ViewData["Claims"] = claims;
                return View(claim);
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, uploadedFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            claim.LecturerId = lecturer.Id;
            claim.LecturerName = $"{lecturer.FirstName} {lecturer.LastName}";
            claim.HourlyRate = lecturer.HourlyRate;
            claim.FileName = uploadedFile.FileName;
            claim.Status = "Pending";

            await _claimRepo.AddClaimAsync(claim);

            TempData["Success"] = "Claim submitted successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Download(string fileName)
        {
            if (HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(fileName))
                return NotFound();

            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/octet-stream", fileName);
        }
    }
}
