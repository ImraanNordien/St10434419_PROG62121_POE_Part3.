using System.Linq;
using System.Threading.Tasks;
using PROG62121_POE.Models;
using PROG62121_POE.Services;
using Xunit;

namespace PROG62121_POE.Tests
{
    public class ClaimRepositoryTests
    {
        [Fact]
        public async Task AddClaim_AssignsId_AndIsRetrievable()
        {
            var repo = new InMemoryClaimRepository();
            var claim = new Claim { LecturerName = "TestUser", HoursWorked = 5, HourlyRate = 100 };
            await repo.AddClaimAsync(claim);

            var all = await repo.GetAllClaimsAsync();
            Assert.Single(all);
            Assert.Equal("TestUser", all.First().LecturerName);
        }

        [Fact]
        public async Task GetClaimsByLecturer_ReturnsOnlyThatLecturerClaims()
        {
            var repo = new InMemoryClaimRepository();
            await repo.AddClaimAsync(new Claim { LecturerId = 1, LecturerName = "A" });
            await repo.AddClaimAsync(new Claim { LecturerId = 2, LecturerName = "B" });

            var aClaims = await repo.GetClaimsByLecturerAsync(1);
            Assert.Single(aClaims);
            Assert.Equal("A", aClaims.First().LecturerName);
        }

        [Fact]
        public async Task UpdateClaim_ChangesStatus()
        {
            var repo = new InMemoryClaimRepository();
            var claim = new Claim { LecturerName = "C" };
            await repo.AddClaimAsync(claim);

            claim.Status = "Verified";
            await repo.UpdateClaimAsync(claim);

            var updated = await repo.GetClaimByIdAsync(claim.ClaimId);
            Assert.Equal("Verified", updated.Status);
        }

        [Fact]
        public async Task MultipleAdds_AssignsUniqueIds()
        {
            var repo = new InMemoryClaimRepository();
            var c1 = new Claim { LecturerName = "X" };
            var c2 = new Claim { LecturerName = "Y" };
            await repo.AddClaimAsync(c1);
            await repo.AddClaimAsync(c2);

            Assert.NotEqual(c1.ClaimId, c2.ClaimId);
        }

        [Fact]
        public async Task GetAllClaims_ReturnsCorrectCount()
        {
            var repo = new InMemoryClaimRepository();
            await repo.AddClaimAsync(new Claim { LecturerName = "M" });
            await repo.AddClaimAsync(new Claim { LecturerName = "N" });

            var all = await repo.GetAllClaimsAsync();
            Assert.Equal(2, all.Count);
        }
    }
}
