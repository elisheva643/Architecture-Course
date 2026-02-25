using server.Models;
using server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.BLL.Interface
{
    public interface IGiftBLL
    {
        Task AddGiftDescriptionToGift(int giftId, GiftDescription giftDescription);
        Task<Gift> Create(GiftDTO gift);
        Task Delete(int id);
        Task<List<Gift>> Get();
        Task<Gift> Get(int id);
        Task<List<Gift>> GetGiftByDoner(int donerId);
        Task<List<Gift>> GetGiftByName(string name);
        Task<int> GetNumberOfPurchases(int giftId);
        Task<Gift> Update(GiftDTO gift);
        Task<List<Gift>> GetGiftByDonerName(string name);
    }
}