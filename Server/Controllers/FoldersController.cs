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
using Folder = Server.Models.Folder;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly DBContext _context;

        public FoldersController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Folders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Folder>>> GetFolders()
        {
            return await _context.Folders.ToListAsync();
        }

        // GET: api/Folders
        [HttpGet("root/{userId}")]
        public async Task<ActionResult<Folder>> GetUserRootFolder(int userId)
        {
            return _context.Folders.Include(f => f.Folders).Include(f => f.Files).ToListAsync().Result.Where(u => u.UserId == userId && u.IsRoot).First();
        }

        [HttpGet("user_folders/{userId}")]
        public async Task<ActionResult<IEnumerable<Folder>>> GetUsersFolders(int userId)
        {
            return _context.Folders.Include(f => f.Folders).Include(f => f.Files).ToList().Where(f => f.UserId == userId).ToList();
        }

        // GET: api/Folders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Folder>> GetFolder(int id)
        {
            var folder = _context.Folders.Include(f => f.Folders).Include(f => f.Files).ToList().Where(f => f.Id == id).First();
            if (folder == null)
            {
                return NotFound();
            }

            return folder;
        }

        // PUT: api/Folders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFolder(int id, string name)
        {
            Folder folder = await _context.Folders.FindAsync(id);
            if (folder == null)
            {
                return BadRequest("No such folder");
            }

            folder.Name = name;
            _context.Entry(folder).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FolderExists(id))
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

        [HttpPost("move")]
        public async Task<ActionResult<Folder>> MoveFolder(int id, int newParentId)
        {
            Folder folder = _context.Folders.Include(f => f.Folders).ToList().Where(f => f.Id == id).First();
            Folder newParentFolder = _context.Folders.Include(f => f.Folders).ToList().Where(f => f.Parent.Id == newParentId).First();

            string oldPath = $"{Constants.PathToRootFolders}\\{folder.Path}";
            string newPath = $"{Constants.PathToRootFolders}\\{newParentFolder.Path}";

            Directory.Move(oldPath, newPath);

            folder.Parent = newParentFolder;
            _context.Entry(folder).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FolderExists(id))
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

        // POST: api/Folders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Folder>> PostFolder(string name, int parentFolderId)
        {
            Folder parentFolder = await _context.Folders.Include(f => f.User).Include(f => f.Parent).Where(f => f.Id == parentFolderId).FirstAsync();
            Folder folder = new Folder()
            {
                Name = name,
                UserId = parentFolder.UserId,
                Parent = parentFolder,
                IsRoot = false,
                Modified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            };

            string path = $"{Constants.PathToRootFolders}\\{parentFolder.Path}\\{name}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFolder", new { id = folder.Id }, folder);
        }

        // DELETE: api/Folders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            var folder = await _context.Folders.Include(f => f.User).Include(f => f.Parent).FirstAsync(f => f.Id == id);

            if (folder == null)
            {
                return NotFound();
            }

            Directory.Delete(Path.Combine(Constants.PathToRootFolders, folder.Path), true);

            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool FolderExists(int id)
        {
            return _context.Folders.Any(e => e.Id == id);
        }

        private string GetFolderName(string filePath)
        {
            return Path.GetFileName(filePath);
        }
    }
}
