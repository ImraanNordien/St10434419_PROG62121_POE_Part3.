using Microsoft.AspNetCore.Mvc;
using PROG62121_POE.Models;
using PROG62121_POE.Services;
using System.IO;
using System.Threading.Tasks;

namespace PROG62121_POE.Controllers
{
    public class LecturerController : Controller
    {
        private readonly IClaimRepository _claimRepo;
        private readonly IWebHostEnvironment _env;

        public LecturerController(IClaimRepository claimRepo, IWebHostEnvironment env)
        {
            _claimRepo = claimRepo;
            _env = env;
        }

        // Display form + submitted claims
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var claims = await _claimRepo.GetAllClaimsAsync();
            ViewData["Claims"] = claims;
            return View(new Claim()); // empty form model
        }

        // Handle form submission
        [HttpPost]
        public async Task<IActionResult> Index(Claim claim, IFormFile uploadedFile)
        {
            if (string.IsNullOrWhiteSpace(claim.LecturerName) ||
                claim.HoursWorked <= 0 ||
                claim.HourlyRate <= 0 ||
                uploadedFile == null)
            {
                TempData["Error"] = "Please fill all fields and upload a file.";
                var existingClaims = await _claimRepo.GetAllClaimsAsync();
                ViewData["Claims"] = existingClaims;
                return View(claim);
            }

            // Save file
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, uploadedFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            claim.FileName = uploadedFile.FileName;
            claim.Status = "Pending";

            await _claimRepo.AddClaimAsync(claim);

            TempData["Success"] = "Claim submitted successfully!";
            return RedirectToAction("Index");
        }


        // Optional: download uploaded files
        [HttpGet]
        public IActionResult Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return NotFound();

            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/octet-stream", fileName);
        }
    }
}








