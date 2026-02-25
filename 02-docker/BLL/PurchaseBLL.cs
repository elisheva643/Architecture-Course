using AutoMapper;
using Microsoft.EntityFrameworkCore;
using server.BLL.Interface;
using server.DAL;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.BLL
{
    public class PurchaseBLL : IPurchaseBLL
    {
        private readonly IPurchaseDAL _purchaseDAL;
        private readonly IMapper mapper;
        private readonly ChineseOrderContext chineseOrderContext;


        public PurchaseBLL(IPurchaseDAL purchaseDAL, IMapper mapper, ChineseOrderContext chineseOrderContext)
        {
            _purchaseDAL = purchaseDAL;
            this.mapper = mapper;
            this.chineseOrderContext = chineseOrderContext;
        }

        public Task<List<Purchase>> Get(int giftId)
        {
            return _purchaseDAL.Get(giftId);
        }

        public async Task<List<PurchaseDTO>> GetExpensive()
        {
            return await chineseOrderContext.Purchases
               
                .Select(p => new PurchaseDTO
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    GiftId = p.GiftId,
                    Quantity = p.Quantity,
                    IsDraft = p.IsDraft
                    // שים לב: אנחנו לא מכניסים כאן אובייקט Gift שלם, 
                    // ולכן הלולאה האינסופית נפתרת!
                })
                .ToListAsync();
        }

        public Task<List<Gift>> GetMostPurchased()
        {
            return _purchaseDAL.GetMostPurchased();
        }

        public Task<List<User>> GetUsers()
        {
            return _purchaseDAL.GetUsers();
        }
        public Task<List<Purchase>> GetPurchaseByUserId(int userId)
        {
            return _purchaseDAL.GetPurchaseByUserId(userId);
        }
        public async Task<Purchase> Create(PurchaseDTO purchaseDto)
        {
            if (await chineseOrderContext.Winners.AnyAsync())
                throw new InvalidOperationException("לא ניתן לבצע רכישות לאחר ההגרלה.");

            var purchase = mapper.Map<Purchase>(purchaseDto);
            return await _purchaseDAL.Create(purchase);
        }

        public async Task<Purchase> Update(PurchaseDTO purchaseDto)
        {
            
            var purchase = mapper.Map<Purchase>(purchaseDto);
            return await _purchaseDAL.Update(purchase);
        }

        public async Task Delete(int purchaseId)
        {
            await _purchaseDAL.Delete(purchaseId);
        }
        public async Task<List<Purchase>> GetCartByUserId(int userId)
        {
            return await _purchaseDAL.GetDraftPurchasesByUserId(userId);
        }
        public async Task<bool> ConfirmCart(int userId)
        {
            if (await chineseOrderContext.Winners.AnyAsync())
                throw new InvalidOperationException("לא ניתן לבצע רכישות לאחר ההגרלה.");

            return await _purchaseDAL.ConfirmCart(userId);
        }
        public async Task<bool> CheckLottery()
        {
            return await _purchaseDAL.CheckLottery();
        }

    }
}