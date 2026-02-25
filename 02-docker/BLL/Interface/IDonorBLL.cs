using server.Models;
using server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.BLL.Interface
{
    public interface IDonorBLL
    {
        Task AddGiftToDoner(int donerId, GiftDescription giftDescription);
        Task<Donor> Create(DonorDTO doner);
        Task Delete(int id);
        Task<List<Donor>> Get();
        Task<Donor> Get(int id);
        Task<List<Donor>> GetDonerByEmail(string email);
        Task<List<Donor>> GetDonerByGift(int giftId);
        Task<List<Donor>> GetDonerByName(string name);
        Task<GiftDescription> GetGiftDescription(int donerId, int giftDescriptionId);
        Task RemoveGiftFromDoner(int donerId, int giftDescriptionId);
        Task<Donor> Update(DonorDTO doner);
    }
}