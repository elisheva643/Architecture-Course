using server.Models;

namespace server.BLL.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}