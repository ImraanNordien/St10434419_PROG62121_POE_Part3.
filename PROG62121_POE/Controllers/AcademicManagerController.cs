using Microsoft.AspNetCore.Mvc;
using PROG62121_POE.Models;
using PROG62121_POE.Services;
using System.Threading.Tasks;

namespace PROG62121_POE.Controllers
{
    public class AcademicManagerController : Controller
    {
        private readonly IClaimRepository _claimRepo;

        public AcademicManagerController(IClaimRepository claimRepo)
        {
            _claimRepo = claimRepo;
        }

        // Display all pending claims
        public async Task<IActionResult> Index()
        {
            var claims = await _claimRepo.GetAllClaimsAsync();
            return View(claims);
        }

        // Approve claim
        [HttpPost]
        public async Task<IActionResult> Approve(int claimId)
        {
            var claim = await _claimRepo.GetClaimByIdAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
                await _claimRepo.UpdateClaimAsync(claim);
            }
            return RedirectToAction("Index");
        }

        // Reject claim
        [HttpPost]
        public async Task<IActionResult> Reject(int claimId)
        {
            var claim = await _claimRepo.GetClaimByIdAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Rejected";
                await _claimRepo.UpdateClaimAsync(claim);
            }
            return RedirectToAction("Index");
        }
    }
}



