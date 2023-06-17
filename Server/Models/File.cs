using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class File
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string? Modified { get; set; }

        public int FolderId { get; set; }

        [JsonIgnore]
        public Folder? Folder { get; set; }

        public File()
        {

        }
    }
}
