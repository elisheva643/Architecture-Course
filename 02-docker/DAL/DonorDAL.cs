using server.DAL.Interface;
using server.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace server.DAL
{
    public class DonorDAL: IDonorDAL
    {
        private readonly ChineseOrderContext chineseOrderContext;

        public DonorDAL(ChineseOrderContext chineseOrderContext)
        {
            this.chineseOrderContext = chineseOrderContext;
        }

        public async Task<List<Donor>> Get()
        {
            return await chineseOrderContext.Doners.ToListAsync();
        }

        public async Task<Donor> Get(int id)
        {
            return await chineseOrderContext.Doners
                .Include(d => d.GiftDescriptions)
                .ThenInclude(gd => gd.Gift)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Donor> Create(Donor doner)
        {
            var result = await chineseOrderContext.Doners.AddAsync(doner);
            await chineseOrderContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Donor> Update(Donor doner)
        {
            var result = chineseOrderContext.Doners.Update(doner);
            await chineseOrderContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task Delete(int id)
        {
            var doner = await chineseOrderContext.Doners.FindAsync(id);
            if (doner != null)
            {
                chineseOrderContext.Doners.Remove(doner);
                await chineseOrderContext.SaveChangesAsync();
            }
        }

        public async Task AddGiftToDoner(int donerId, GiftDescription giftDescription)
        {
            var doner = await chineseOrderContext.Doners
                .Include(d => d.GiftDescriptions)
                .FirstOrDefaultAsync(d => d.Id == donerId);

            if (doner != null)
            {
                giftDescription.DonerId = donerId;
                doner.GiftDescriptions.Add(giftDescription);
                await chineseOrderContext.SaveChangesAsync();
            }
        }

        public async Task RemoveGiftFromDoner(int donerId, int giftDescriptionId)
        {
            var doner = await chineseOrderContext.Doners
                .Include(d => d.GiftDescriptions)
                .FirstOrDefaultAsync(d => d.Id == donerId);

            if (doner != null)
            {
                var giftToRemove = doner.GiftDescriptions
                    .FirstOrDefault(gd => gd.Id == giftDescriptionId);
                if (giftToRemove != null)
                {
                    doner.GiftDescriptions.Remove(giftToRemove);
                    await chineseOrderContext.SaveChangesAsync();
                }
            }
        }

        public async Task<GiftDescription> GetGiftDescription(int donerId, int giftDescriptionId)
        {
            return await chineseOrderContext.GiftDescriptions
                .FirstOrDefaultAsync(gd => gd.Id == giftDescriptionId && gd.DonerId == donerId);
        }

        public async Task<List<Donor>> GetDonerByName(string name)
        {
            return await chineseOrderContext.Doners
                .Where(d => d.Name == name)
                .ToListAsync();
        }

        public async Task<List<Donor>> GetDonerByEmail(string email)
        {
            return await chineseOrderContext.Doners
                .Where(d => d.Email == email)
                .ToListAsync();
        }

        public async Task<List<Donor>> GetDonerByGift(int giftId)
        {
            return await chineseOrderContext.Doners
                .Where(d => d.GiftDescriptions.Any(gd => gd.GiftId == giftId))
                .Distinct()
                .ToListAsync();
        }

    }
}