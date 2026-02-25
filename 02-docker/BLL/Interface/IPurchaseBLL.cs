using server.Models;
using server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.BLL.Interface
{
    public interface IPurchaseBLL
    {
        Task<List<Purchase>> Get(int giftId);
        Task<List<PurchaseDTO>> GetExpensive();
        Task<List<Gift>> GetMostPurchased();
        Task<List<User>> GetUsers();
        Task<List<Purchase>> GetPurchaseByUserId(int userId);
        Task<Purchase> Create(PurchaseDTO purchaseDto);
        Task<Purchase> Update(PurchaseDTO purchaseDto);
        Task Delete(int purchaseId);
        Task<List<Purchase>> GetCartByUserId(int userId);
        Task<bool> ConfirmCart(int userId);
        Task<bool> CheckLottery();



    }
}