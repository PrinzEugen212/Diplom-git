namespace PcClient.Models.LocalModels
{
    public class Filters
    {
        public bool OnlyFolders { get; set; }

        public bool OnlyFiles { get; set; }

        public long SizeValue { get; set; }

        public SizeComparsionType SizeComparsion { get; set; }

        public SortType SortType { get; set; }

        public SortDirection SortDirection { get; set; }
    }
}
