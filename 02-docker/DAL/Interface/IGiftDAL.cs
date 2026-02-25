using server.Models;

namespace server.DAL.Interface
{
    public interface IGiftDAL
    {
        Task AddGiftDescriptionToGift(int giftId, GiftDescription giftDescription);
        Task<Gift> Create(Gift gift);
        Task Delete(int id);
        Task<List<Gift>> Get();
        Task<Gift> Get(int id);
        Task<List<Gift>> GetGiftByDoner(int donerId);
        Task<List<Gift>> GetGiftByName(string name);
        Task<int> GetNumberOfPurchases(int giftId);
        Task<Gift> Update(Gift gift);
        Task<List<Gift>> GetGiftByDonerName(string donerName);
    }
}