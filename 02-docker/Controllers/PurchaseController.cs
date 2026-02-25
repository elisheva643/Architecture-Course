using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.BLL;
using server.BLL.Interface;
using server.DAL;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseBLL purchaseBLL;
        public PurchaseController(IPurchaseBLL purchaseBLL)
        {
            this.purchaseBLL = purchaseBLL;
        }

        [HttpGet("by-gift/{giftId}")]
        public async Task<IActionResult> Get(int giftId)
        {
            var purchase = await purchaseBLL.Get(giftId);
            if (purchase == null || !purchase.Any())
                return NotFound();
            return Ok(purchase);
        }

        [HttpGet("get-expensive")]
        public async Task<IActionResult> GetExpensive()
        {
            var purchases = await purchaseBLL.GetExpensive();
            if (purchases == null || !purchases.Any())
                return NotFound();
            return Ok(purchases);
        }

        [HttpGet("get-most-purchased")]
        public async Task<IActionResult> GetMostPurchased()
        {
            var purchases = await purchaseBLL.GetMostPurchased();
            if (purchases == null || !purchases.Any())
                return NotFound();
            return Ok(purchases);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var purchases = await purchaseBLL.GetUsers();
            if (purchases == null || !purchases.Any())
                return NotFound();
            return Ok(purchases);
        }
        [HttpGet("by-user/{user_id}")]
        public async Task<IActionResult> GetPurchaseByUserId(int user_id)
        {
            var purchases = await purchaseBLL.GetPurchaseByUserId(user_id);
            if (purchases == null || !purchases.Any())
                return NotFound();
            return Ok(purchases);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseDTO purchaseDto)
        {
            try
            {
                if (purchaseDto == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdPurchase = await purchaseBLL.Create(purchaseDto);

                return Ok(createdPurchase);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PurchaseDTO purchaseDto)
        {
            if (purchaseDto == null || id != purchaseDto.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedPurchase = await purchaseBLL.Update(purchaseDto);
            if (updatedPurchase == null)
                return NotFound();

            return Ok(updatedPurchase);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await purchaseBLL.Delete(id);
            return NoContent(); 
        }
        [HttpGet("cart/{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await purchaseBLL.GetCartByUserId(userId);

            if (cart == null)
            {
                return Ok(new List<Purchase>());
}

            return Ok(cart);
        }
        [HttpPost("confirm/{userId}")]
        public async Task<IActionResult> ConfirmPurchase(int userId)
        {
            try
            {
                var result = await purchaseBLL.ConfirmCart(userId);

                if (!result)
                    return BadRequest("העגלה ריקה או שלא נמצאו פריטים לאישור");

                return Ok(new { Message = "הרכישה אושרה בהצלחה!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאת שרת: {ex.Message}");
            }
        }
        [HttpGet("checkLottery")]
        public async Task<IActionResult> CheckLottery()
        {
            var result = await purchaseBLL.CheckLottery();
            return Ok(result);
        }

    }
}