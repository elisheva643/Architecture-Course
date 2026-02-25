using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using server.BLL.Interface;
using server.DAL.Interface;
using server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.BLL
{
    public class WinnerBLL : IWinnerBLL
    {
        private readonly IWinnerDAL winnerDAL;
        private readonly Random rnd = new Random();

        public WinnerBLL(IWinnerDAL winnerDAL)
        {
            this.winnerDAL = winnerDAL;
        }

        public async Task<List<Winner>> Winner()
        {
            var purchases = await winnerDAL.GetPurchases();
            var allEntries = new List<(int GiftId, int UserId)>();

            foreach (var purchase in purchases)
            {
                for (int i = 0; i < purchase.Quantity; i++)
                    allEntries.Add((purchase.GiftId, purchase.UserId));
            }

            var winnersDict = allEntries
                .GroupBy(e => e.GiftId)
                .ToDictionary(
                    g => g.Key,
                    g => g.ElementAt(rnd.Next(g.Count())).UserId
                );

            var currentWinners = await winnerDAL.GetWinner();
            var results = new List<Winner>();

            foreach (var kvp in winnersDict)
            {
                var existingWinner = currentWinners.FirstOrDefault(w => w.GiftId == kvp.Key);
                if (existingWinner == null)
                {
                    Winner winner = new Winner
                    {
                        GiftId = kvp.Key,
                        UserId = kvp.Value
                    };
                    await winnerDAL.AddWinner(winner);
                    results.Add(winner);
                }
                else
                {
                    results.Add(existingWinner);
                }
            }

            return results; 
        }
        public async Task<List<Winner>> GetWinners()
        {
            return await winnerDAL.GetWinner();
        }
        public async Task DeleteAllWinners()
        {
            await winnerDAL.ClearWinners();
        }
    }
}