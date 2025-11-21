using PROG62121_POE.Models;
using System.Collections.Generic;

namespace PROG62121_POE.Services
{
    public interface IUserService
    {
        void AddUser(User user);
        void UpdateUser(User user);
        User GetUserById(int id);
        User GetUserByEmail(string email);
        IEnumerable<User> GetAllUsers();
    }
}
