using Microsoft.AspNetCore.Mvc;
using PROG62121_POE.Models;
using PROG62121_POE.Services;

namespace PROG62121_POE.Controllers
{
    public class ProgrammeCoordinatorController : Controller
    {
        private readonly IClaimRepository _claimRepository;

        public ProgrammeCoordinatorController(IClaimRepository claimRepository)
        {
            _claimRepository = claimRepository;
        }

        // GET: Display all pending claims
        public async Task<IActionResult> Index()
        {
            var claims = (await _claimRepository.GetAllClaimsAsync())
                         .Where(c => c.Status == "Pending")
                         .ToList();

            return View(claims);
        }

        // POST: Verify or Reject a claim
        [HttpPost]
        public async Task<IActionResult> VerifyClaim(int claimId, string action)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(claimId);
            if (claim == null)
                return NotFound();

            if (action == "verify")
                claim.Status = "Verified";
            else if (action == "reject")
                claim.Status = "Rejected";

            await _claimRepository.UpdateClaimAsync(claim);
            return RedirectToAction("Index");
        }
    }
}
