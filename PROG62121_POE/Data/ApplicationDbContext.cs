using Microsoft.EntityFrameworkCore;
using PROG62121_POE.Models;

namespace PROG62121_POE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<User> Users { get; set; } // HR, Lecturer, Coordinator, Manager
    }
}
