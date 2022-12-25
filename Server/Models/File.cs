using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class File
    {
        public string Name { get; set; }

        public uint Size { get; set; }

        public string? Modified { get; set; }

        public int Id { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public File()
        {

        }
    }
}
