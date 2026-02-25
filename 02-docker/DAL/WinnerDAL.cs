using server.DAL.Interface;
using server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.DAL
{
    public class WinnerDAL : IWinnerDAL
    {
        private readonly ChineseOrderContext chineseOrderContext;

        public WinnerDAL(ChineseOrderContext chineseOrderContext)
        {
            this.chineseOrderContext = chineseOrderContext;
        }

        public async Task<List<Purchase>> GetPurchases()
        {
            return await chineseOrderContext.Purchases.ToListAsync();
        }

        public async Task<List<Gift>> GetGifts()
        {
            return await chineseOrderContext.Gifts.ToListAsync();
        }

        public async Task AddWinner(Winner winner)
        {
            await chineseOrderContext.Winners.AddAsync(winner);
            await chineseOrderContext.SaveChangesAsync();
        }

        public async Task<List<Winner>> GetWinner()
        {
            return await chineseOrderContext.Winners
                .Include(w => w.Gift)
                .Include(w => w.User)
                .ToListAsync();
        }
        public async Task ClearWinners()
        {
            var allWinners = await chineseOrderContext.Winners.ToListAsync();
            chineseOrderContext.Winners.RemoveRange(allWinners);
            await chineseOrderContext.SaveChangesAsync();
        }
    }
}
