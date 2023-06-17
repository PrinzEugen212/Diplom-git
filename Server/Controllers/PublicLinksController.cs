using Microsoft.AspNetCore.Mvc;
using Server.Context;
using Server.Models;
using File = Server.Models.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Models;
using System.IO.Compression;

namespace Server.Controllers
{
    [Route("public")]
    [ApiController]
    public class PublicLinksController : ControllerBase
    {
        private readonly DBContext _context;

        public PublicLinksController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> LinkExists(bool isFile, int contentID)
        {
            PublicLink link = await _context.PublicLinks.Where(p => p.ContentID == contentID && p.IsFile == isFile).FirstOrDefaultAsync();
            if (link is not null)
            {
                return Ok(link);
            }
            else
            { 
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<PublicLink>> CreateLink(bool isFile, int contentID, int downloadCount = -1)
        {
            if (isFile)
            {
                PublicLink link = await _context.PublicLinks.Where(p => p.ContentID == contentID && p.IsFile == isFile).FirstOrDefaultAsync();
                if (link is not null) 
                {
                    return Ok(link);
                }

                File file = await _context.Files.FindAsync(contentID);
                if (file is null)
                {
                    return NotFound();
                }

                link = new PublicLink() 
                {
                    ID = Guid.NewGuid().ToString(),
                    ContentID = contentID,
                    IsFile = isFile,
                    DownloadCount = downloadCount,
                };

                if (downloadCount > 0)
                {
                    link.IsDownloadCountRestricted = true;
                }

                _context.Add(link);
                _context.SaveChanges();
                return Ok(link);
            }
            else
            {

                PublicLink link = await _context.PublicLinks.Where(p => p.ContentID == contentID && p.IsFile == isFile).FirstOrDefaultAsync();
                if (link is not null)
                {
                    return Ok(link);
                }

                Folder folder = await _context.Folders.FindAsync(contentID);
                if (folder is null)
                {
                    return NotFound();
                }

                link = new PublicLink()
                {
                    ID = Guid.NewGuid().ToString(),
                    ContentID = contentID,
                    IsFile = isFile,
                };

                _context.Add(link);
                _context.SaveChanges();
                return Ok(link);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Download(string id)
        {
            PublicLink publicLink = await _context.PublicLinks.FindAsync(id);
            if (publicLink is null)
            {
                return NotFound();
            }

            if (publicLink.IsDownloadCountRestricted)
            {
                if (publicLink.DownloadCount - 1 < 0)
                {
                    _context.PublicLinks.Remove(publicLink);
                    await _context.SaveChangesAsync();
                    return NotFound();
                }

                publicLink.DownloadCount--;
                _context.Entry(publicLink).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            if (publicLink.IsFile)
            {
                File file = await _context.Files.FindAsync(publicLink.ContentID);
                if (file is null)
                {
                    return NotFound();
                }
                Folder folder = _context.Folders.Include(f => f.Folders).ToList().First(f => f.Id == file.FolderId);
                string filePath = Path.Combine(Constants.PathToRootFolders, folder.Path, file.Name);

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(bytes, contentType, Path.GetFileName(filePath));
            }
            else
            {
                Folder folder = _context.Folders.Include(f => f.Folders).ToList().First(f => f.Id == publicLink.ContentID);
                string folderPath = Path.Combine(Constants.PathToRootFolders, folder.Path);

                int count = 0;
                string startName = "folder";
                string zipPath = Path.Combine(Constants.PathToRootFolders, $"{startName}.zip");
                while (System.IO.File.Exists(zipPath))
                {
                    startName = $"folder{count++}";
                    zipPath = Path.Combine(Constants.PathToRootFolders, $"{startName}.zip");
                }

                ZipFile.CreateFromDirectory(folderPath, zipPath, CompressionLevel.SmallestSize, true);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(folderPath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var bytes = await System.IO.File.ReadAllBytesAsync(zipPath);
                System.IO.File.Delete(zipPath);
                return File(bytes, contentType, Path.GetFileName(zipPath));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(bool isFile, int contentID)
        {
            PublicLink publicLink = await _context.PublicLinks.Where(p => p.ContentID == contentID && p.IsFile == isFile).FirstOrDefaultAsync();
            if (publicLink is null)
            {
                return NotFound();
            }

            _context.PublicLinks.Remove(publicLink);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
