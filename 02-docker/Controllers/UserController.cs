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
    public class UserController : ControllerBase
    {
        private readonly IUserBLL userBLL;
        private readonly IJwtService jwtService;

        public UserController(IUserBLL userBLL, IJwtService jwtService)
        {
            this.userBLL = userBLL;
            this.jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            try
            {
                var user = await userBLL.Create(register);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var user = await userBLL.GetUserByEmail(login.Email, login.Password);
            if (user == null)
                return Unauthorized();

            var token = jwtService.GenerateToken(user);
            return Ok(new { token, user });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userBLL.DeleteUser(id);
            if (!result)
                return NotFound("משתמש לא נמצא");

            return Ok("המשתמש נמחק בהצלחה");
        }
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await userBLL.GetAllUsers();
            return Ok(users);
        }
    }
}