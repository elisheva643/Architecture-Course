using AutoMapper;
using server.DAL.Interface;
using server.Models.DTO;
using server.Models;
using server.BLL.Interface;

namespace server.BLL
{
    public class GiftDescriptionBLL : IGiftDescriptionBLL
    {
        private readonly IGiftDescriptionDAL giftDescriptionDAL;
        private readonly IMapper mapper;

        public GiftDescriptionBLL(
            IGiftDescriptionDAL giftDescriptionDAL,
            IMapper mapper)
        {
            this.giftDescriptionDAL = giftDescriptionDAL;
            this.mapper = mapper;
        }

        public async Task<GiftDescription> Create(GiftDescriptionDTO dto)
        {
            var entity = mapper.Map<GiftDescription>(dto);
            return await giftDescriptionDAL.Create(entity);
        }
    }
}
