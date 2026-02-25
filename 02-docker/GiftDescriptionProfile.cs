using AutoMapper;
using server.Models.DTO;
using server.Models;

namespace server
{
    public class GiftDescriptionProfile : Profile
    {
        public GiftDescriptionProfile()
        {
            CreateMap<GiftDescription, GiftDescriptionDTO>().ReverseMap();
        }
    

    }
}
