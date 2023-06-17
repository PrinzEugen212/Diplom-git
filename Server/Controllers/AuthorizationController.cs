using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly DBContext _context;

        public AuthorizationController(DBContext context)
        {
            _context = context;
        }

        [HttpGet("authorize")]
        public async Task<ActionResult<User>> GetUser(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null)
            {
                return NoContent();
            }

            if (user.Password != password)
            {
                return Unauthorized();
            }

            return user;
        }

        [HttpGet("login_exists")]
        public async Task<ActionResult<bool>> LoginExists(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login.Equals(login));
            if (user == null)
            {
                return false;
            }

            return true;
        }

        [HttpGet("email_exists")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
