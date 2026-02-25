using server.Models;

namespace server.DAL.Interface
{
    public interface IUserDAL
    {
        Task<User> Create(User user);
        Task<User> GetByEmail(string email);
        Task<bool> Delete(int id);
        Task<List<User>> GetAll();
    }
}