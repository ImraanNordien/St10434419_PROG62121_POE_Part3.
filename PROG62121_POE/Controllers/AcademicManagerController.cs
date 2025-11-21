using Microsoft.AspNetCore.Mvc;
using PROG62121_POE.Models;
using PROG62121_POE.Services;
using System.Threading.Tasks;
using System.Linq;

namespace PROG62121_POE.Controllers
{
    public class AcademicManagerController : Controller
    {
        private readonly IClaimRepository _claimRepo;

        public AcademicManagerController(IClaimRepository claimRepo)
        {
            _claimRepo = claimRepo;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "AcademicManager")
                return RedirectToAction("Login", "Account");

            var claims = (await _claimRepo.GetAllClaimsAsync())
                         .Where(c => c.Status == "Verified")
                         .ToList();

            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int claimId)
        {
            if (HttpContext.Session.GetString("Role") != "AcademicManager")
                return RedirectToAction("Login", "Account");

            var claim = await _claimRepo.GetClaimByIdAsync(claimId);
            if (claim != null && claim.Status == "Verified")
            {
                claim.Status = "Approved";
                await _claimRepo.UpdateClaimAsync(claim);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int claimId)
        {
            if (HttpContext.Session.GetString("Role") != "AcademicManager")
                return RedirectToAction("Login", "Account");

            var claim = await _claimRepo.GetClaimByIdAsync(claimId);
            if (claim != null && claim.Status == "Verified")
            {
                claim.Status = "Rejected";
                await _claimRepo.UpdateClaimAsync(claim);
            }
            return RedirectToAction("Index");
        }
    }
}
