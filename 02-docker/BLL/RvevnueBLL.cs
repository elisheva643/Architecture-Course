using server.BLL.Interface;
using server.DAL;
using server.DAL.Interface;
using System.Threading.Tasks;

namespace server.BLL
{
    public class RvevnueBLL : IRvevnueBLL
    {
        private readonly IRvevnueDAL _rvevnueDAL;

        public RvevnueBLL(IRvevnueDAL rvevnueDAL)
        {
            _rvevnueDAL = rvevnueDAL;
        }

        public Task<decimal> GetRvevnue()
        {
            return _rvevnueDAL.GetRvevnue();
        }
    }
}