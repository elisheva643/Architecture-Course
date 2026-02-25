using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Interface;
using server.DAL;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftBLL giftBLL;
        public GiftController(IGiftBLL giftBLL)
        {
            this.giftBLL = giftBLL;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var gifts = await giftBLL.Get();
            if (gifts == null || !gifts.Any())
                return NotFound();
            return Ok(gifts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var gift = await giftBLL.Get(id);
            if (gift == null)
                return NotFound();
            return Ok(gift);
        }
        [Authorize(Roles = "Manager")]

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GiftDTO gift)
        {
            if (gift == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var createdGift = await giftBLL.Create(gift);
            return CreatedAtAction(nameof(Get), new { id = createdGift.Id }, createdGift);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GiftDTO gift)
        {
            if (gift == null || id != gift.Id)
                return BadRequest();

            var updatedGift = await giftBLL.Update(gift);
            if (updatedGift == null)
                return NotFound();

            return Ok(updatedGift);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var gift = await giftBLL.Get(id);
                if (gift == null)
                    return NotFound("המתנה לא נמצאה במערכת.");

                await giftBLL.Delete(id);
                return NoContent(); // מחזיר הצלחה (204)
            }
            catch (Exception ex)
            {
                // תופס את ה-Exception שזרקת ב-BLL ומחזיר את ההודעה למשתמש
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("get-name")]
        public async Task<IActionResult> GetGiftByName([FromQuery] string name)
        {
            if (name == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var gits = await giftBLL.GetGiftByName(name);
            if (gits == null)
                return NotFound();

            return Ok(gits);
        }

        [HttpGet("get-gift-doner/{donerId}")]
        public async Task<IActionResult> GetGiftByDoner([FromRoute] int donerId)
        {
            var gifts = await giftBLL.GetGiftByDoner(donerId);
            if (gifts == null || !gifts.Any())
                return NotFound();

            return Ok(gifts);
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
            if (gifts == null || !gifts.Any())
                return NotFound();

            return Ok(gifts);
        }

    }
}