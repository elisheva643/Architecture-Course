using Microsoft.AspNetCore.Identity;
using server.BLL.Interface;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.Threading.Tasks;

namespace server.BLL
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserDAL _userDAL;

        public UserBLL(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public async Task<User> Create(RegisterDTO register)
        {
            var user = new User
            {
                Email = register.Email,
                Name = register.Name
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, register.Password);

            await _userDAL.Create(user);
            return user;
        }

        public async Task<User> GetUserByEmail(string email, string password)
        {
            var user = await _userDAL.GetByEmail(email);
            if (user == null)
                return null;

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Success)
                return user;

            return null;
        }
        public async Task<bool> DeleteUser(int id)
        {
            return await _userDAL.Delete(id);
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _userDAL.GetAll();
        }
    }
}