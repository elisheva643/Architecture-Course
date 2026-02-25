using server.DAL.Interface;
using server.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.DAL
{
    public class GiftDAL : IGiftDAL
    {
        private readonly ChineseOrderContext chineseOrderContext;

        public GiftDAL(ChineseOrderContext chineseOrderContext)
        {
            this.chineseOrderContext = chineseOrderContext;
        }

        public async Task<List<Gift>> Get()
        {
            return await chineseOrderContext.Gifts.ToListAsync();
        }

        public async Task<Gift> Get(int id)
        {
            return await chineseOrderContext.Gifts
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Gift> Create(Gift gift)
        {
            var result = await chineseOrderContext.Gifts.AddAsync(gift);
            await chineseOrderContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Gift> Update(Gift gift)
        {
            var result = chineseOrderContext.Gifts.Update(gift);
            await chineseOrderContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task Delete(int id)
        {
            var gift = await chineseOrderContext.Gifts
                .Include(g => g.Purchases)
                .Include(g => g.GiftDescriptions)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gift == null)
                throw new Exception("המתנה לא קיימת.");

            if (gift.Purchases != null && gift.Purchases.Any())
                throw new Exception("לא ניתן למחוק מתנה שנרכשו עבורה כרטיסים.");

            if (gift.GiftDescriptions != null && gift.GiftDescriptions.Any())
                throw new Exception("יש למחוק את שיוך התורם למתנה לפני מחיקת המתנה עצמה.");

            chineseOrderContext.Gifts.Remove(gift);
            await chineseOrderContext.SaveChangesAsync();
        }

        public async Task AddGiftDescriptionToGift(int giftId, GiftDescription giftDescription)
        {
            giftDescription.GiftId = giftId;
            await chineseOrderContext.GiftDescriptions.AddAsync(giftDescription);
            await chineseOrderContext.SaveChangesAsync();
        }

        public async Task<List<Gift>> GetGiftByName(string name)
        {
            return await chineseOrderContext.Gifts
                .Where(g => g.Name == name)
                .ToListAsync();
        }

        public async Task<List<Gift>> GetGiftByDoner(int donerId)
        {
            return await chineseOrderContext.Gifts
                .Where(g => g.GiftDescriptions.Any(gd => gd.DonerId == donerId))
                .Distinct()
                .ToListAsync();
        }

        public async Task<int> GetNumberOfPurchases(int giftId)
        {
            return await chineseOrderContext.Purchases
                .Where(p => p.GiftId == giftId && !p.IsDraft)
                .SumAsync(p => p.Quantity);
        }

        public async Task<List<Gift>> GetGiftByDonerName(string donerName)
        {
            return await chineseOrderContext.Gifts
                .Where(g => g.GiftDescriptions.Any(gd => gd.Doner.Name == donerName))
                .Distinct()
                .ToListAsync();
        }

    }
}