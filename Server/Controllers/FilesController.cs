using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Models;
using File = Server.Models.File;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly string pathToRootFolders = "E:\\Diplom(\\Server\\files";

        public FilesController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Files
        [HttpGet]
        public async Task<ActionResult<IEnumerable<File>>> GetFiles()
        {
            return await _context.Files.ToListAsync();
        }

        // GET: api/Files
        [HttpGet("user_files/{userId}")]
        public async Task<ActionResult<IEnumerable<File>>> GetUsersFiles(int userId)
        {
            return await _context.Files.Where(u => u.UserId == userId).ToListAsync();
        }

        // GET: api/Files/5
        [HttpGet("{id}")]
        public async Task<ActionResult<File>> GetFile(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpGet("download/{id}")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            var file = _context.Files.Include(f => f.User).First(f => f.Id == id);
            string filePath = Path.Combine(pathToRootFolders, file.User.Login, file.Name);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }

        // PUT: api/Files/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFile(int id, int userId)
        {
            IFormFile attachedFile = Request.Form.Files.FirstOrDefault();
            if (attachedFile is null || attachedFile.Length <= 0)
            {
                return BadRequest("Empty file");
            }

            File file = new File()
            {
                Id = id,
                UserId = userId,
                Size = (uint)attachedFile.Length / 1024,
                Name = attachedFile.FileName,
                Modified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            User user = _context.Users.Where(u => u.Id == file.UserId).FirstOrDefault();
            string path = $"{pathToRootFolders}\\{user.Login}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, attachedFile.FileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await attachedFile.CopyToAsync(stream);
            }

            _context.Entry(file).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(id))
                {
                    System.IO.File.Delete(filePath);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Files
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<File>> PostFile(int userId)
        {
            IFormFile attachedFile = Request.Form.Files.FirstOrDefault();
            if (attachedFile is null || attachedFile.Length <= 0)
            {
                return BadRequest("Empty file");
            }

            File file = new File()
            {
                UserId = userId,
                Size = (uint)attachedFile.Length / 1024,
                Name = attachedFile.FileName,
                Modified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            User user = _context.Users.Where(u => u.Id == file.UserId).FirstOrDefault();
            string path = $"{pathToRootFolders}\\{user.Login}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, attachedFile.FileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await attachedFile.CopyToAsync(stream);
            }

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFile", new { id = file.Id }, file);
        }

        // DELETE: api/Files/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _context.Files.Include(f => f.User).FirstAsync(f => f.Id == id);

            if (file == null)
            {
                return NotFound();
            }

            string filePath = Path.Combine(pathToRootFolders, file.User.Login, file.Name);
            System.IO.File.Delete(filePath);

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool FileExists(int id)
        {
            return _context.Files.Any(e => e.Id == id);
        }

        private string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }
    }
}
