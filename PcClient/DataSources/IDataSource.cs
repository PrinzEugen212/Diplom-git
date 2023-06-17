using PcClient.DataSources.DataSourceUtils;
using PcClient.Models;
using Server.Models;
using static PcClient.DataSources.DataSourceUtils.HttpClientDownloadWithProgress;

namespace PcClient.DataSources
{
    public interface IDataSource
    {
        public Task<Folder> GetUserFiles(string userID);

        public Task<User> Authorize(string login, string password);

        public Task<bool> Register(User user);

        public Task<bool> LoginExists(string login);

        public Task<bool> EmailExists(string email);

        public Task<bool> ChangeUser(User user);

        public Task<User> GetUser(string userID);

        public void DownloadFile(string fileID, string filePath, ProgressChangedHandler? handler = null);

        public Task<bool> AddFile(string folderID, string fileName, Stream fileStream, ProgressHandler handler);

        public Task<bool> ChangeFile(string fileName, string fileID, Stream fileStream, ProgressHandler handler);

        public Task<bool> DeleteFile(string fileID);

        public Task<Folder> AddFolder(string name, string parentFolderID);

        public Task<bool> ChangeFolder(string folderID, string folderName);

        public Task<bool> DeleteFolder(string folderID);

        public Task<PublicLink> LinkExists(string contentID, bool isFile);

        public Task<bool> DeleteLink(string contentID, bool isFile);

        public Task<PublicLink> CreateLink(string contentID, bool isFile, string? downloadCount = null);
    }
}
