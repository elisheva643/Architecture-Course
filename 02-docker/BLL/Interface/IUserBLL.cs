using server.Models;
using server.Models.DTO;

namespace server.BLL.Interface
{
    public interface IUserBLL
    {
        Task<User> Create(RegisterDTO register);
        Task<User> GetUserByEmail(string email, string password);
        Task<bool> DeleteUser(int id);
        Task<List<User>> GetAllUsers();
    }
}