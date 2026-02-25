using server.Models;
using server.Models.DTO;

namespace server.BLL.Interface
{
    public interface IGiftDescriptionBLL
    {
        Task<GiftDescription> Create(GiftDescriptionDTO dto);
    }
}