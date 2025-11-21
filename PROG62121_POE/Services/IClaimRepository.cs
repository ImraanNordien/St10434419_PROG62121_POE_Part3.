using PROG62121_POE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PROG62121_POE.Services
{
    public interface IClaimRepository
    {
        Task<List<Claim>> GetAllClaimsAsync();
        Task<Claim> GetClaimByIdAsync(int id);
        Task AddClaimAsync(Claim claim);

        // ✅ Added methods
        Task<List<Claim>> GetClaimsByLecturerIdAsync(int lecturerId);
        Task UpdateClaimAsync(Claim claim);

        Task<List<Claim>> GetClaimsByLecturerAsync(int lecturerId);


    }

}
