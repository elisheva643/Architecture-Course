using server.Models;

namespace server.DAL.Interface
{
    public interface IGiftDescriptionDAL
    {
        Task<GiftDescription> Create(GiftDescription giftDescription);
    }
}