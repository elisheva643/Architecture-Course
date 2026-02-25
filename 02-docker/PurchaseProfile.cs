using AutoMapper;
using server.Models.DTO;
using server.Models;

namespace server
{
    public class PurchaseProfile:Profile
    {
        public PurchaseProfile()
        {
            CreateMap<Purchase, PurchaseDTO>().ReverseMap();
        }
    }
}
