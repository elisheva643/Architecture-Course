using server.Models.DTO;
using server.Models;
using AutoMapper;

namespace server
{
    public class WinnerProfile : Profile
    {
        public WinnerProfile()
        {
            CreateMap<Winner, WinnerDTO>().ReverseMap();
        }
    }
}
