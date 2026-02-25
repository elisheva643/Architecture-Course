using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Interface;
using server.Models.DTO;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftDescriptionController:ControllerBase
    {
        private readonly IGiftDescriptionBLL giftDescriptionBLL;

        public GiftDescriptionController(IGiftDescriptionBLL giftDescriptionBLL)
        {
            this.giftDescriptionBLL = giftDescriptionBLL;
        }
        [Authorize(Roles = "Manager")]

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] GiftDescriptionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await giftDescriptionBLL.Create(dto);
            return Ok(created);
        }
    }
}
