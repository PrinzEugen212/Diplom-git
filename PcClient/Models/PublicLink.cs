namespace Server.Models
{
    public class PublicLink
    {
        public string ID { get; set; }

        public bool IsFile { get; set; }

        public int ContentID { get; set; }

        public bool IsDownloadCountRestricted { get; set; }

        public int DownloadCount { get; set; }
    }
}
