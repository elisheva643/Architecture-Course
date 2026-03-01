using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using server.BLL.Interface;
using server.Models;
using server.Models.DTO;
using System.Text.Json;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftBLL giftBLL;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "all_gifts_list";

        public GiftController(IGiftBLL giftBLL, IDistributedCache cache)
        {
            this.giftBLL = giftBLL;
            this._cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cachedGifts = await _cache.GetStringAsync(CacheKey);

            if (!string.IsNullOrEmpty(cachedGifts))
            {
                var giftsFromCache = JsonSerializer.Deserialize<IEnumerable<Gift>>(cachedGifts);
                return Ok(giftsFromCache);
            }

            var gifts = await giftBLL.Get();

            if (gifts == null || !gifts.Any())
                return NotFound();

            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            var serializedGifts = JsonSerializer.Serialize(gifts);
            await _cache.SetStringAsync(CacheKey, serializedGifts, cacheOptions);

            return Ok(gifts);
        }

        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromBody] GiftDTO gift)
        {
            if (gift == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var createdGift = await giftBLL.Create(gift);

            await _cache.RemoveAsync(CacheKey);

            return CreatedAtAction(nameof(Get), new { id = createdGift.Id }, createdGift);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] GiftDTO gift)
        {
            if (gift == null || id != gift.Id)
                return BadRequest();

            var updatedGift = await giftBLL.Update(gift);
            if (updatedGift == null)
                return NotFound();

            await _cache.RemoveAsync(CacheKey);

            return Ok(updatedGift);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var gift = await giftBLL.Get(id);
                if (gift == null)
                    return NotFound("המתנה לא נמצאה במערכת.");

                await giftBLL.Delete(id);

                await _cache.RemoveAsync(CacheKey);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var gift = await giftBLL.Get(id);
            if (gift == null) return NotFound();
            return Ok(gift);
        }

        [HttpGet("get-name")]
        public async Task<IActionResult> GetGiftByName([FromQuery] string name)
        {
            var gifts = await giftBLL.GetGiftByName(name);
            return gifts == null ? NotFound() : Ok(gifts);
        }

        [HttpGet("get-gift-doner/{donerId}")]
        public async Task<IActionResult> GetGiftByDoner([FromRoute] int donerId)
        {
            var gifts = await giftBLL.GetGiftByDoner(donerId);
            return (gifts == null || !gifts.Any()) ? NotFound() : Ok(gifts);
        }

        [HttpGet("get-number/{giftId}")]
        public async Task<IActionResult> GetNumberOfPurchases([FromRoute] int giftId)
        {
            var number = await giftBLL.GetNumberOfPurchases(giftId);
            return Ok(number);
        }

        [HttpGet("get-gift-doner-name")]
        public async Task<IActionResult> GetGiftByDonerName([FromQuery] string name)
        {
            var gifts = await giftBLL.GetGiftByDonerName(name);
            return (gifts == null || !gifts.Any()) ? NotFound() : Ok(gifts);
        }
    }
}