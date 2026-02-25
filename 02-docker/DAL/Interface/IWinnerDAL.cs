using server.Models;

namespace server.DAL.Interface
{
    public interface IWinnerDAL
    {
        Task<List<Purchase>> GetPurchases();
        Task<List<Gift>> GetGifts();
        Task AddWinner(Winner winner);
        Task<List<Winner>> GetWinner();
        Task ClearWinners();
        //Task Winner();
    }
}