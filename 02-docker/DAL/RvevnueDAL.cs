using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace server.DAL
{
    public class RvevnueDAL : IRvevnueDAL
    {
        private readonly ChineseOrderContext chineseOrderContext;

        public RvevnueDAL(ChineseOrderContext chineseOrderContext)
        {
            this.chineseOrderContext = chineseOrderContext;
        }

        public async Task<decimal> GetRvevnue()
        {
            return await chineseOrderContext.Purchases
                .Where(p => !p.IsDraft)
                .Select(p => p.Gift.Price * p.Quantity)
                .SumAsync();
        }
    }
}
