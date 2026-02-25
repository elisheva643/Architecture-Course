using server.DAL.Interface;
using server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DAL
{
    public class PurchaseDAL : IPurchaseDAL
    {
        private readonly ChineseOrderContext chineseOrderContext;

        public PurchaseDAL(ChineseOrderContext chineseOrderContext)
        {
            this.chineseOrderContext = chineseOrderContext;
        }

        public async Task<List<Purchase>> Get(int giftId)
        {
            return await chineseOrderContext.Purchases
                .Where(p => p.GiftId == giftId)
                .ToListAsync();
        }

        public async Task<List<Purchase>> GetExpensive()
        {
            // במקום להחזיר Gifts, אנחנו מחזירים את טבלת ה-Purchases (הרכישות)
            return await chineseOrderContext.Purchases
                .Include(p => p.Gift)  // מביא את שם ומחיר המתנה
                .Include(p => p.User)  // מביא את פרטי המשתמש
                .OrderByDescending(p => p.Gift.Price) // מיון לפי מחיר המתנה
                .ToListAsync();
        }
  
       

        public async Task<List<Gift>> GetMostPurchased()
        {
            return await chineseOrderContext.Gifts
                .OrderByDescending(g => g.Purchases.Count)
                .ToListAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await chineseOrderContext.Purchases
                .Select(p => p.User)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Purchase>> GetPurchaseByUserId(int userId)
        {
            return await chineseOrderContext.Purchases
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Purchase> Create(Purchase purchase)
        {
            await chineseOrderContext.Purchases.AddAsync(purchase);
            await chineseOrderContext.SaveChangesAsync();
            return purchase;
        }

        public async Task<Purchase> Update(Purchase purchase)
        {
            chineseOrderContext.Purchases.Update(purchase);
            await chineseOrderContext.SaveChangesAsync();
            return purchase;
        }

        public async Task Delete(int purchaseId)
        {
            var purchase = await chineseOrderContext.Purchases
                .FirstOrDefaultAsync(p => p.Id == purchaseId);

            if (purchase == null)
                throw new Exception("הרכישה לא נמצאה");

            if (!purchase.IsDraft)
                throw new Exception("לא ניתן למחוק רכישה שכבר בוצעה!");

            chineseOrderContext.Purchases.Remove(purchase);
            await chineseOrderContext.SaveChangesAsync();
        }

        public async Task<List<Purchase>> GetDraftPurchasesByUserId(int userId)
        {
            return await chineseOrderContext.Purchases
                .Where(p => p.UserId == userId && p.IsDraft)
                .ToListAsync();
        }

        public async Task<bool> ConfirmCart(int userId)
        {
            var userCart = await chineseOrderContext.Purchases
                .Where(p => p.UserId == userId && p.IsDraft)
                .ToListAsync();

            if (!userCart.Any())
                return false;

            userCart.ForEach(p => p.IsDraft = false);
            await chineseOrderContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckLottery()
        {
            return await chineseOrderContext.Winners.AnyAsync();
        }
    }
}
