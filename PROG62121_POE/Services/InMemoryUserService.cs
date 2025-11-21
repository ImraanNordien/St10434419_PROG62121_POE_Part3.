using PROG62121_POE.Models;
using System.Collections.Generic;
using System.Linq;

namespace PROG62121_POE.Services
{
    public class InMemoryUserService : IUserService
    {
        private readonly List<User> _users = new();

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public void UpdateUser(User user)
        {
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.FirstName = user.FirstName;
                existing.LastName = user.LastName;
                existing.Email = user.Email;
                existing.Role = user.Role;
                existing.HourlyRate = user.HourlyRate;
            }
        }

        public User GetUserById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public User GetUserByEmail(string email) => _users.FirstOrDefault(u => u.Email == email);

        public IEnumerable<User> GetAllUsers() => _users;
    }
}
