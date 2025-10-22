using PROG62121_POE.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG62121_POE.Services
{
    public class InMemoryClaimRepository : IClaimRepository
    {
        private readonly ConcurrentDictionary<int, Claim> _claims = new();
        private int _nextId = 1;

        public Task<List<Claim>> GetAllClaimsAsync()
            => Task.FromResult(_claims.Values.ToList());

        public Task<List<Claim>> GetClaimsByLecturerAsync(int lecturerId)
            => Task.FromResult(_claims.Values.Where(c => c.LecturerId == lecturerId).ToList());

        public Task<Claim?> GetClaimByIdAsync(int claimId)
        {
            _claims.TryGetValue(claimId, out var claim);
            return Task.FromResult(claim);
        }

        public Task AddClaimAsync(Claim claim)
        {
            claim.ClaimId = _nextId++;
            _claims.TryAdd(claim.ClaimId, claim);
            return Task.CompletedTask;
        }

        public Task UpdateClaimAsync(Claim claim)
        {
            _claims[claim.ClaimId] = claim;
            return Task.CompletedTask;
        }
    }
}
