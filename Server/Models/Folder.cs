using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Folder
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Modified { get; set; }

        public int UserId { get; set; }

        public bool IsRoot { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Folder? Parent { get; set; }

        public List<File> Files { get; set; }

        public List<Folder> Folders { get; set; }

        [NotMapped]
        public string Path
        {
            get
            {
                if (IsRoot)
                {
                    return Name;
                }

                return $"{Parent.Path}\\{Name}";
            }
        }

        public Folder()
        {

        }
    }
}
