using AutoMapper;
using server.Models.DTO;
using server.Models;

namespace server
{
    public class GiftProfile:Profile
    {
        public GiftProfile()
        {
            CreateMap<Gift, GiftDTO>().ReverseMap();
        }
    }
}
