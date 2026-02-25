

using AutoMapper;
using server.Models;
using server.Models.DTO;

namespace server

{
    public class DonorProfile:Profile
    {
        public DonorProfile() {
            CreateMap<Donor, DonorDTO>().ReverseMap();
        }
    }
}
