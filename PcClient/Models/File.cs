namespace PcClient.Models
{
    public class File
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string? Modified { get; set; }

        public int FolderId { get; set; }

        public Folder? Folder { get; set; }

        public File()
        {

        }
    }
}
