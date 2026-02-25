using server.DAL.Interface;
using server.Models;

namespace server.DAL
{
    public class GiftDescriptionDAL : IGiftDescriptionDAL
    {
        private readonly ChineseOrderContext chineseOrderContext;

        public GiftDescriptionDAL(ChineseOrderContext chineseOrderContext)
        {
            this.chineseOrderContext = chineseOrderContext;
        }
        public async Task<GiftDescription> Create(GiftDescription giftDescription)
        {
            var result = await chineseOrderContext.GiftDescriptions.AddAsync(giftDescription);
            await chineseOrderContext.SaveChangesAsync();
            return result.Entity;
        }
    }
}
