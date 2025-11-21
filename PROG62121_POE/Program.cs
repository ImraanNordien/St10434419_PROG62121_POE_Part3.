using Microsoft.EntityFrameworkCore;
using PROG62121_POE.Data;
using PROG62121_POE.Services;

namespace PROG62121_POE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // In-memory repository for claims (from Part 2)
            builder.Services.AddSingleton<IClaimRepository, InMemoryClaimRepository>();

            // EF Core DbContext for Part 3 (SQL Server)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // User service for HR/Users management
            builder.Services.AddSingleton<IUserService, InMemoryUserService>();

            // Add Session support for login/authentication
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();       // Session must be before Authorization
            app.UseAuthorization();

            // Default route: opens Login page
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            // Other routes if needed (Lecturer, HR, Coordinator, Manager)
            app.MapControllerRoute(
                name: "hr",
                pattern: "{controller=HR}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "lecturer",
                pattern: "{controller=Lecturer}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "pc",
                pattern: "{controller=ProgrammeCoordinator}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "am",
                pattern: "{controller=AcademicManager}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated(); // Creates DB if missing
                DatabaseSeeder.Seed(dbContext);     // Seeds initial users
            }

            app.Run();
        }
    }
}
