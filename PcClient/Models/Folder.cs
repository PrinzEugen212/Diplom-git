namespace PcClient.Models
{
    public class Folder
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Modified { get; set; }

        public int UserId { get; set; }

        public bool IsRoot { get; set; }

        public User? User { get; set; }

        public Folder? Parent { get; set; }

        public List<File> Files { get; set; }

        public List<Folder> Folders { get; set; }

        public string Path { get; set; }

        public Folder()
        {

        }
    }
}
