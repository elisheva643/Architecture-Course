using AutoMapper;
using server.BLL.Interface;
using server.DAL;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.BLL
{
    public class GiftBLL : IGiftBLL
    {
        private readonly IGiftDAL giftDAL;
        private readonly IMapper mapper;

        public GiftBLL(IGiftDAL giftDAL, IMapper mapper)
        {
            this.giftDAL = giftDAL;
            this.mapper = mapper;
        }

        public async Task<List<Gift>> Get()
        {
            return await giftDAL.Get();
        }

        public async Task<Gift> Get(int id)
        {
            return await giftDAL.Get(id);
        }

        public async Task<Gift> Create(GiftDTO giftd)
        {
            var gift = mapper.Map<Gift>(giftd);
            return await giftDAL.Create(gift);
        }

        public async Task<Gift> Update(GiftDTO giftd)
        {
            var gift = mapper.Map<Gift>(giftd);
            return await giftDAL.Update(gift);
        }

        public async Task Delete(int id)
        {
            var gift = await giftDAL.Get(id);

            if (gift == null)
                throw new Exception("המתנה לא קיימת.");

            if (gift.Purchases != null && gift.Purchases.Any())
            {
                throw new Exception("לא ניתן למחוק מתנה שנרכשו עבורה כרטיסים.");
            }

            if (gift.GiftDescriptions != null && gift.GiftDescriptions.Any())
            {
                throw new Exception("יש למחוק את שיוך התורם למתנה לפני מחיקת המתנה עצמה.");
            }

            await giftDAL.Delete(id);
        }

        public async Task AddGiftDescriptionToGift(int giftId, GiftDescription giftDescription)
        {
            await giftDAL.AddGiftDescriptionToGift(giftId, giftDescription);
        }

        public async Task<List<Gift>> GetGiftByName(string name)
        {
            return await giftDAL.GetGiftByName(name);
        }

        public async Task<List<Gift>> GetGiftByDoner(int donerId)
        {
            return await giftDAL.GetGiftByDoner(donerId);
        }

        public async Task<int> GetNumberOfPurchases(int giftId)
        {
            return await giftDAL.GetNumberOfPurchases(giftId);
        }
        public async Task<List<Gift>> GetGiftByDonerName(string name)
        {
            return await giftDAL.GetGiftByDonerName(name);
        }

    }
}