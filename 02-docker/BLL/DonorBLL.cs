using AutoMapper;
using server.BLL.Interface;
using server.DAL;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.BLL
{
    public class DonorBLL : IDonorBLL
    {
        private readonly IDonorDAL donerDal;
        private readonly IMapper mapper;

        public DonorBLL(IDonorDAL donerDal, IMapper mapper)
        {
            this.donerDal = donerDal;
            this.mapper = mapper;
        }

        public async Task<List<Donor>> Get()
        {
            return await donerDal.Get();
        }

        public async Task<Donor> Get(int id)
        {
            return await donerDal.Get(id);
        }

        public async Task<Donor> Create(DonorDTO donerd)
        {
            var doner = mapper.Map<Donor>(donerd);
            return await donerDal.Create(doner);
        }

        public async Task<Donor> Update(DonorDTO donerd)
        {
            var doner = mapper.Map<Donor>(donerd);
            return await donerDal.Update(doner);
        }

        public async Task Delete(int id)
        {
            var donor = await donerDal.Get(id);

            if (donor == null)
            {
                throw new Exception("התורם לא קיים.");
            }

            if (donor.GiftDescriptions != null && donor.GiftDescriptions.Any())
            {
               
                throw new Exception("לא ניתן למחוק תורם שיש לו מתנות פעילות.");
            }
            await donerDal.Delete(id);
        }

        public async Task AddGiftToDoner(int donerId, GiftDescription giftDescription)
        {
            await donerDal.AddGiftToDoner(donerId, giftDescription);
        }

        public async Task RemoveGiftFromDoner(int donerId, int giftDescriptionId)
        {
            await donerDal.RemoveGiftFromDoner(donerId, giftDescriptionId);
        }

        public async Task<GiftDescription> GetGiftDescription(int donerId, int giftDescriptionId)
        {
            return await donerDal.GetGiftDescription(donerId, giftDescriptionId);
        }

        public async Task<List<Donor>> GetDonerByName(string name)
        {
            return await donerDal.GetDonerByName(name);
        }

        public async Task<List<Donor>> GetDonerByEmail(string email)
        {
            return await donerDal.GetDonerByEmail(email);
        }

        public async Task<List<Donor>> GetDonerByGift(int giftId)
        {
            return await donerDal.GetDonerByGift(giftId);
        }
    }
}