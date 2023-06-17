using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;

        public UsersController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            User oldUser = await _context.Users.FindAsync(id);
            if (oldUser.Login != user.Login)
            {
                Folder folder = await _context.Folders.Where(f => f.UserId == id && f.IsRoot).FirstAsync();
                string oldPath = $"{Constants.PathToRootFolders}\\{folder.Name}";
                folder.Name = user.Login;
                string newPath = $"{Constants.PathToRootFolders}\\{folder.Name}";
                Directory.Move(oldPath, newPath);
                _context.Entry(folder).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();
            }

            _context.ChangeTracker.Clear();
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            string path = $"{Constants.PathToRootFolders}\\{user.Login}";
            Folder folder = new Folder()
            {
                Name = user.Login,
                IsRoot = true,
                Modified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            };
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            folder.User = user;
            _context.Users.Add(user);
            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            string path = $"{Constants.PathToRootFolders}\\{user.Login}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
