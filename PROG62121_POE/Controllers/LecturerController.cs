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

            // ✅ NEW: Validate hours range
            if (claim.HoursWorked <= 0 || claim.HoursWorked > 180)
            {
                TempData["Error"] = "Hours worked must be between 1 and 180.";
                var claims = await _claimRepo.GetClaimsByLecturerIdAsync(lecturer.Id);
                ViewData["Claims"] = claims;
                return View(claim);
            }

            // ✅ NEW: Check for file upload
            if (uploadedFile == null)
            {
                TempData["Error"] = "Please upload a supporting document.";
                var claims = await _claimRepo.GetClaimsByLecturerIdAsync(lecturer.Id);
                ViewData["Claims"] = claims;
                return View(claim);
            }

            // ✅ NEW: Validate file type and size
            var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".png", ".jpg" };
            var extension = Path.GetExtension(uploadedFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                TempData["Error"] = "Invalid file type. Only PDF, Word, Excel, PNG, and JPG files are allowed.";
                var claims = await _claimRepo.GetClaimsByLecturerIdAsync(lecturer.Id);
                ViewData["Claims"] = claims;
                return View(claim);
            }

            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (uploadedFile.Length > maxFileSize)
            {
                TempData["Error"] = "File too large. Maximum size allowed is 5 MB.";
                var claims = await _claimRepo.GetClaimsByLecturerIdAsync(lecturer.Id);
                ViewData["Claims"] = claims;
                return View(claim);
            }

            // ✅ NEW: Safer file naming
            var sanitizedFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
            sanitizedFileName = $"{sanitizedFileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}";

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, sanitizedFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            // ✅ Populate claim details safely
            claim.LecturerId = lecturer.Id;
            claim.LecturerName = $"{lecturer.FirstName} {lecturer.LastName}";
            claim.HourlyRate = lecturer.HourlyRate;
            claim.FileName = sanitizedFileName;
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
