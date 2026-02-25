using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Interface;
using server.Models.DTO;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WinnerController : ControllerBase
    {
        private readonly IWinnerBLL winnerBLL;
        public WinnerController(IWinnerBLL winnerBLL)
        {
            this.winnerBLL = winnerBLL;
        }
      
       [Authorize(Roles = "Manager")]
[HttpPost("draw")]
public async Task<IActionResult> DrawWinners()
{
    try
    {
        // קריאה ללוגיקת ההגרלה ב-BLL
        var results = await winnerBLL.Winner();

        // בדיקה אם חזרו תוצאות (למקרה שאין רכישות בכלל)
        if (results == null || !results.Any())
        {
            return NotFound("לא נמצאו רכישות לביצוע הגרלה.");
        }

        // המרה ל-DTO בצורה בטוחה
        var resultsDto = results.Select(w => new WinnerDTO
        {
            Id = w.Id,
            GiftName = w.Gift?.Name ?? "מתנה ללא שם",
            UserName = w.User?.Name ?? "משתמש ללא שם"
        }).ToList();

        return Ok(resultsDto);
    }
    catch (Exception ex)
    {
        // במקרה של תקלה (מסד נתונים, שרת וכו')
        // מומלץ לרשום את השגיאה ב-Log (אם יש לך Logger)
        return StatusCode(500, $"שגיאת שרת פנימית בזמן ביצוע ההגרלה: {ex.Message}");
    }
}

        [HttpGet("draw")]
        public async Task<IActionResult> GetWinners()
        {
            var results = await winnerBLL.GetWinners();

            var resultsDto = results.Select(w => new WinnerDTO
            {
                Id = w.Id,
                GiftName = w.Gift.Name,
                UserName = w.User.Name
            }).ToList();

            return Ok(resultsDto);
        }
        //[Authorize(Roles = "Manager")]
        [HttpDelete("reset")]
        public async Task<IActionResult> ResetLottery()
        {
            await winnerBLL.DeleteAllWinners();
            return NoContent(); 
        }

    }
}