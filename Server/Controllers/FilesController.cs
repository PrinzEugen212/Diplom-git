using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
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
using Server.Utils;
using static NuGet.Packaging.PackagingConstants;
using File = Server.Models.File;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly DBContext _context;

        public FilesController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Files
        [HttpGet]
        public async Task<ActionResult<IEnumerable<File>>> GetFiles()
        {
            return await _context.Files.Include(f => f.Folder).ToListAsync();
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
            File file = await _context.Files.FindAsync(id);
            if (file is null)
            {
                return BadRequest("No such file");
            }

            Folder folder = _context.Folders.Include(f => f.Folders).ToList().First(f => f.Id == file.FolderId);
            User user = _context.Users.Find(folder.Id);
            string filePath = Path.Combine(Constants.PathToRootFolders, folder.Path, file.Name);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            Stream output = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await DecryptStream(stream, output, user.Password);
            }
            var bytes = ReadFully(output);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }

        [DisableFormValueModelBinding]
        [RequestSizeLimit(Constants.MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = Constants.MaxFileSize)]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(int folderId)
        {
            var boundary = RequestHelpers.GetBoundary(Request.ContentType);
            var reader = new MultipartReader(boundary, Request.Body);
            var section = await reader.ReadNextSectionAsync();
            var fileSection = section.AsFileSection();

            Folder parent = _context.Folders.Include(f => f.Folders).ToList().Where(f => f.Id == folderId).First();
            User user = _context.Users.Find(parent.UserId);
            Request.EnableBuffering();
            File file = new File()
            {
                Size = Request.Body.Length,
                Name = fileSection.FileName,
                Modified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Folder = parent,
                FolderId = parent.Id,
            };

            string path = $"{Constants.PathToRootFolders}\\{parent.Path}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, fileSection.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.Size = await EncryptStream(fileSection.FileStream, stream, user.Password);
            }

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [DisableFormValueModelBinding]
        [RequestSizeLimit(Constants.MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = Constants.MaxFileSize)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFile(int id)
        {
            var boundary = RequestHelpers.GetBoundary(Request.ContentType);
            var reader = new MultipartReader(boundary, Request.Body);
            var section = await reader.ReadNextSectionAsync();

            var fileSection = section.AsFileSection();

            File file = await _context.Files.FirstAsync(f => f.Id == id);
            if (file is null)
            {
                return BadRequest("File not found");
            }

            Folder parent = _context.Folders.Include(f => f.Folders).ToList().Where(f => f.Id == file.FolderId).First();
            Request.EnableBuffering();
            file.Size = (uint)Request.Body.Length;
            file.Modified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (file.Name != fileSection.FileName)
            {
                string deletePath = $"{Constants.PathToRootFolders}\\{parent.Path}\\{file.Name}";
                System.IO.File.Delete(deletePath);
                file.Name = fileSection.FileName;
            }

            User user = parent.User;
            string filePath = $"{Constants.PathToRootFolders}\\{parent.Path}\\{file.Name}";
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                const int chunkSize = 10240;
                var buffer = new byte[chunkSize];
                var bytesRead = 0;
                do
                {
                    bytesRead = await fileSection.FileStream.ReadAsync(buffer, 0, buffer.Length);
                    file.Size += bytesRead;
                    await stream.WriteAsync(buffer.AsMemory(0, bytesRead));
                } while (bytesRead > 0);
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

        [HttpPost("move")]
        public async Task<ActionResult<Folder>> MoveFile(int id, int newParentId)
        {
            File file = await _context.Files.FindAsync(id);
            if (file is null)
            {
                return BadRequest("No such file");
            }

            Folder folder = _context.Folders.Include(f => f.Folders).ToList().First(f => f.Id == file.FolderId);
            Folder newParentFolder = _context.Folders.Include(f => f.Folders).ToList().Where(f => f.Parent.Id == newParentId).First();

            string oldPath = $"{Constants.PathToRootFolders}\\{folder.Path}\\{file.Name}";
            string newPath = $"{Constants.PathToRootFolders}\\{newParentFolder.Path}\\{file.Name}";

            System.IO.File.Move(oldPath, newPath);

            file.FolderId = newParentId;
            _context.Entry(file).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(id))
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

        // DELETE: api/Files/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            File file = await _context.Files.FindAsync(id);
            if (file is null)
            {
                return BadRequest("No such file");
            }

            Folder folder = _context.Folders.Include(f => f.Folders).ToList().First(f => f.Id == file.FolderId);

            if (folder == null)
            {
                return NotFound();
            }

            string filePath = Path.Combine(Constants.PathToRootFolders, folder.Path, file.Name);
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

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
        {
            public void OnResourceExecuting(ResourceExecutingContext context)
            {
                var factories = context.ValueProviderFactories;
                factories.RemoveType<FormValueProviderFactory>();
                factories.RemoveType<FormFileValueProviderFactory>();
                factories.RemoveType<JQueryFormValueProviderFactory>();
            }

            public void OnResourceExecuted(ResourceExecutedContext context)
            {
            }
        }

        static async Task<long> EncryptStream(Stream inputStream, Stream outputStream, string key)
        {
            byte[] keyBytes = new byte[32];
            long fileSize = 0;

            // Convert the key string to bytes and truncate or pad if necessary
            byte[] keyBytesSource = Encoding.UTF8.GetBytes(key);
            Array.Copy(keyBytesSource, keyBytes, Math.Min(keyBytesSource.Length, keyBytes.Length));

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.GenerateIV();
                byte[] ivBytes = new byte[16];
                ivBytes = aes.IV;

                // Write the IV to the output stream
                outputStream.Write(ivBytes, 0, ivBytes.Length);

                using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // Encrypt the input stream and write to the output stream
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = await inputStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        fileSize += bytesRead;
                        cryptoStream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            return fileSize;
        }

        static async Task<bool> DecryptStream(Stream inputStream, Stream outputStream, string key)
        {
            byte[] keyBytes = new byte[32];
            byte[] ivBytes = new byte[16];

            byte[] keyBytesSource = Encoding.UTF8.GetBytes(key);
            Array.Copy(keyBytesSource, keyBytes, Math.Min(keyBytesSource.Length, keyBytes.Length));

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                inputStream.Read(ivBytes, 0, ivBytes.Length);
                aes.IV = ivBytes;

                inputStream.Position = 0;

                using (CryptoStream cryptoStream = new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = await cryptoStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            return true;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            input.Position = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
