using server.Models;

namespace server.DAL.Interface
{
    public interface IPurchaseDAL
    {
        Task<List<Purchase>> Get(int giftId);
        Task<List<Purchase>> GetExpensive();
        Task<List<Gift>> GetMostPurchased();
        Task<List<User>> GetUsers();
        Task<Purchase> Create(Purchase purchase);
        Task<Purchase> Update(Purchase purchase);
        Task Delete(int purchaseId);
        Task<List<Purchase>> GetPurchaseByUserId(int userId);
        Task<List<Purchase>> GetDraftPurchasesByUserId(int userId);
        Task<bool> ConfirmCart(int userId);
        Task<bool> CheckLottery();


    }
}