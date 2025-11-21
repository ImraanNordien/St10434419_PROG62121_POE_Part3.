using PROG62121_POE.Models;

namespace PROG62121_POE.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Only seed if there are no users yet
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { FirstName = "Liam", LastName = "Lecturer", Email = "lecturer1@college.com", Password = "pass123", Role = "Lecturer", HourlyRate = 250 },
                    new User { FirstName = "Paula", LastName = "Coordinator", Email = "pc1@college.com", Password = "pass123", Role = "ProgrammeCoordinator", HourlyRate = 0 },
                    new User { FirstName = "Andy", LastName = "Manager", Email = "am1@college.com", Password = "pass123", Role = "AcademicManager", HourlyRate = 0 },
                    new User { FirstName = "Hannah", LastName = "HR", Email = "hr1@college.com", Password = "pass123", Role = "HR", HourlyRate = 0 }
                );

                context.SaveChanges();
            }
        }
    }
}
