using PcClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcClient.Utils
{
    public static class FileSystemUtils
    {
        public static long GetFolderSize(Folder folder)
        {
            long size = 0;
            if (folder is null || (folder.Files.Count == 0) && folder.Folders.Count == 0)
            {
                return size;
            }

            foreach (var file in folder.Files)
            {
                size += file.Size;
            }

            foreach (var subFolder in folder.Folders)
            {
                size += GetFolderSize(subFolder);
            }

            return size;
        }

        public static Folder? FindFolder(Folder rootSearchFolder, int folderID)
        {
            Folder? folder = null;
            if (rootSearchFolder.Id == folderID)
            {
                return rootSearchFolder;
            }

            for (int i = 0; i < rootSearchFolder.Folders.Count; i++)
            {
                if (rootSearchFolder.Folders[i].Id == folderID)
                {
                    folder = rootSearchFolder.Folders[i];
                    return folder;
                }
            }

            if (folder is null)
            {
                for (int i = 0; i < rootSearchFolder.Folders.Count; i++)
                {
                    folder = FindFolder(rootSearchFolder.Folders[i], folderID);
                    if (folder is not null)
                    {
                        return folder;
                    }
                }
            }

            return null;
        }

        public static string GetFilePath(Folder rootSearchFolder, int fileID, string path = "")
        {
            path += $"{rootSearchFolder.Name}\\";

            if (rootSearchFolder.Files is null)
            {
                return string.Empty;
            }

            for (int i = 0; i < rootSearchFolder.Files.Count; i++)
            {
                if (rootSearchFolder.Files[i].Id == fileID)
                {
                    return $"{path}{rootSearchFolder.Files[i].Name}";
                }
            }

            if (rootSearchFolder.Folders is null)
            {
                return string.Empty;
            }

            for (int i = 0; i < rootSearchFolder.Folders.Count; i++)
            {
                string local = GetFilePath(rootSearchFolder.Folders[i], fileID, path);
                if (!string.IsNullOrEmpty(local))
                {
                    return local;
                }
            }

            return string.Empty;
        }

        public static string GetFolderPath(Folder rootSearchFolder, int folderID, string path = "")
        {
            path += $"{rootSearchFolder.Name}\\";

            if (rootSearchFolder.Folders is null)
            {
                return string.Empty;
            }

            for (int i = 0; i < rootSearchFolder.Folders.Count; i++)
            {
                if (rootSearchFolder.Folders[i].Id == folderID)
                {
                    return $"{path}{rootSearchFolder.Folders[i].Name}";
                }
            }

            for (int i = 0; i < rootSearchFolder.Folders.Count; i++)
            {
                string local = GetFolderPath(rootSearchFolder.Folders[i], folderID, path);
                if (!string.IsNullOrEmpty(local))
                {
                    return local;
                }
            }

            return string.Empty;
        }

        public static Folder? FindFolder(Folder rootSearchFolder, string folderName)
        {
            Folder? folder = null;
            if (rootSearchFolder.Name == folderName)
            {
                return rootSearchFolder;
            }

            for (int i = 0; i < rootSearchFolder.Folders.Count; i++)
            {
                if (rootSearchFolder.Folders[i].Name == folderName)
                {
                    folder = rootSearchFolder.Folders[i];
                    return folder;
                }
            }

            if (folder is null)
            {
                for (int i = 0; i < rootSearchFolder.Folders.Count; i++)
                {
                    folder = FindFolder(rootSearchFolder.Folders[i], folderName);
                    if (folder is not null)
                    {
                        return folder;
                    }
                }
            }

            return null;
        }
    }
}
