using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.BLL;
using server.BLL.Interface;
using server.Models.DTO;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RvevnueController : ControllerBase
    {
        private readonly IRvevnueBLL rvevnueBLL;

        public RvevnueController(IRvevnueBLL rvevnueBLL)
        {
            this.rvevnueBLL = rvevnueBLL;
        }
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetRvevnue()
        {
            var rvevnue = await rvevnueBLL.GetRvevnue();
            return Ok(rvevnue);
        }
    }
}