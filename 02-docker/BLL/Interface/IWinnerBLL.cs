using server.Models;

namespace server.BLL.Interface
{
    public interface IWinnerBLL
    {
       Task<List<Winner>> Winner();
        Task<List<Winner>> GetWinners();
        Task DeleteAllWinners();
    }
}