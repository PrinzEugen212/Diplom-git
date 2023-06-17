using PcClient.Models;
using System.Runtime.InteropServices;

namespace PcClient.Utils
{
    public static class GeneralUtils
    {
        public static string FileSizeToString(long fileSize)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double doubleSize = fileSize;
            while (doubleSize >= 1024 && order < sizes.Length - 1)
            {
                order++;
                doubleSize /= 1024;
            }

            return string.Format("{0:0.#} {1}", doubleSize, sizes[order]);
        }

        public static ContextMenuStrip GetContextMenu(EventHandler eventHandler)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Создать публичную ссылку", null, eventHandler);
            return menu;
        }

        public static string GetUniquePath(string directory, string fileName)
        {
            for (int i = 1; System.IO.File.Exists(Path.Combine(directory, fileName)); i++)
            {
                string newName = Path.GetFileNameWithoutExtension(fileName).Replace($"({i})", "");
                fileName = $"{newName}{$" ({i})"}{Path.GetExtension(fileName)}";
            }

            return Path.Combine(directory, fileName);
        }

        private static readonly Dictionary<KnownFolder, Guid> _guids = new()
        {
            [KnownFolder.Contacts] = new("56784854-C6CB-462B-8169-88E350ACB882"),
            [KnownFolder.Downloads] = new("374DE290-123F-4565-9164-39C4925E467B"),
            [KnownFolder.Favorites] = new("1777F761-68AD-4D8A-87BD-30B759FA33DD"),
            [KnownFolder.Links] = new("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968"),
            [KnownFolder.SavedGames] = new("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"),
            [KnownFolder.SavedSearches] = new("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA")
        };

        public static string GetPath(KnownFolder knownFolder)
        {
            return SHGetKnownFolderPath(_guids[knownFolder], 0);
        }

        public static string GetDownloadsFolder()
        {
            return GetPath(KnownFolder.Downloads);
        }

        [DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        private static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, nint hToken = 0);
    }

    public enum KnownFolder
    {
        Contacts,
        Downloads,
        Favorites,
        Links,
        SavedGames,
        SavedSearches
    }
}
