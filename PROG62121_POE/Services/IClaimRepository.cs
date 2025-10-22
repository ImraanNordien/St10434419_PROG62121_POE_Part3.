using PROG62121_POE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PROG62121_POE.Services
{
    public interface IClaimRepository
    {
        Task<List<Claim>> GetAllClaimsAsync();
        Task<List<Claim>> GetClaimsByLecturerAsync(int lecturerId);
        Task<Claim?> GetClaimByIdAsync(int claimId);
        Task AddClaimAsync(Claim claim);
        Task UpdateClaimAsync(Claim claim);
    }
}
