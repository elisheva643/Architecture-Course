using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Interface;
using server.Models;
using server.Models.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorBLL donerBLL;

        public DonorController(IDonorBLL donerBLL)
        {
            this.donerBLL = donerBLL;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doners = await donerBLL.Get();
            if (doners == null || !doners.Any())
                return NotFound();
            return Ok(doners);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var doner = await donerBLL.Get(id);

                if (doner == null)
                    return NotFound();

                doner.GiftDescriptions ??= new List<GiftDescription>();

               
                var donerDto = doner.GiftDescriptions.Select(gd => new
                {
                    gd.Id,
                    gd.Quantity,
                    Gift = new
                    {
                        gd.Gift.Id,
                        gd.Gift.Name,
                        gd.Gift.Description,
                        gd.Gift.Price
                    }
                });

                return Ok(new
                {
                    doner.Id,
                    doner.Name,
                    doner.Email,
                    giftDescription = donerDto
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Server error: " + ex.Message);
            }
        }
        [Authorize(Roles = "Manager")]

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DonorDTO donor)
        {
            if (donor == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDoner = await donerBLL.Create(donor);
            return CreatedAtAction(nameof(Get), new { id = createdDoner.Id }, createdDoner);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DonorDTO donor)
        {
            if (donor == null || id != donor.Id)
                return BadRequest();

            var updatedDonor = await donerBLL.Update(donor);
            if (updatedDonor == null)
                return NotFound();

            return Ok(updatedDonor);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var donor = await donerBLL.Get(id);
                if (donor == null)
                    return NotFound("התורם לא נמצא במערכת.");

                await donerBLL.Delete(id);
                return NoContent(); // מחזיר הצלחה (204)
            }
            catch (Exception ex)
            {
                // מחזיר למשל: "לא ניתן למחוק תורם שיש לו מתנות פעילות."
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-name")]
        public async Task<IActionResult> GetDonerByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var donors = await donerBLL.GetDonerByName(name);
            if (donors == null || !donors.Any())
                return NotFound();
            return Ok(donors);
        }

        [HttpGet("get-email")]
        public async Task<IActionResult> GetDonerByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            var donors = await donerBLL.GetDonerByEmail(email);
            if (donors == null || !donors.Any())
                return NotFound();
            return Ok(donors);
        }

        [HttpGet("get-gift/{giftId}")]
        public async Task<IActionResult> GetDonerByGift([FromRoute] int giftId)
        {
            var donors = await donerBLL.GetDonerByGift(giftId);
            if (donors == null || !donors.Any())
                return NotFound();
            return Ok(donors);
        }
    }
}