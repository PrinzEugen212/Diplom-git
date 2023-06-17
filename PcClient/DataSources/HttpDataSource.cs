using Newtonsoft.Json;
using PcClient.DataSources.DataSourceUtils;
using PcClient.Models;
using Server.Models;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Xml.Linq;
using static PcClient.DataSources.DataSourceUtils.HttpClientDownloadWithProgress;
using User = PcClient.Models.User;

namespace PcClient.DataSources
{
    public class HttpDataSource : IDataSource
    {
        private static readonly HttpClient _client = new HttpClient();

        public HttpDataSource()
        {

        }

        public async Task<Folder> GetUserFiles(string userID)
        {
            string api = $"/api/Folders/root/{userID}";

            string response = await _client.GetStringAsync($"{Constants.ServerUrl}{api}");
            return JsonConvert.DeserializeObject<Folder>(response);
        }

        public async Task<User> Authorize(string login, string password)
        {
            string api = $"/api/Authorization/authorize?login={login}&password={password}";

            string response = await _client.GetStringAsync($"{Constants.ServerUrl}{api}");
            return JsonConvert.DeserializeObject<User>(response);
        }

        public async Task<bool> Register(User user)
        {
            string api = "/api/Users";

            JsonContent content = JsonContent.Create(user);
            var response = await _client.PostAsync($"{Constants.ServerUrl}{api}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LoginExists(string login)
        {
            string api = $"/api/Authorization/login_exists?login={login}";

            string response = await _client.GetStringAsync($"{Constants.ServerUrl}{api}");
            return bool.Parse(response);
        }

        public async Task<bool> EmailExists(string email)
        {
            string api = $"/api/Authorization/email_exists?email={email}";

            string response = await _client.GetStringAsync($"{Constants.ServerUrl}{api}");
            return bool.Parse(response);
        }

        public async Task<bool> ChangeUser(User user)
        {
            string api = $"/api/Users/{user.Id}";

            JsonContent content = JsonContent.Create(user);
            var response = await _client.PutAsync($"{Constants.ServerUrl}{api}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<User> GetUser(string userID)
        {
            string api = $"/api/Users/{userID}";

            string response = await _client.GetStringAsync($"{Constants.ServerUrl}{api}");
            return JsonConvert.DeserializeObject<User>(response);
        }

        public async Task<bool> AddFile(string folderID, string fileName, Stream fileStream, ProgressHandler handler)
        {
            string api = $"/api/Files/upload?folderId={folderID}";
            var streamContent = new ProgressStreamContent(fileStream);
            streamContent.ProgressChanged += (bytesRead) => { handler(bytesRead); };
            var content = new MultipartFormDataContent();
            content.Add(streamContent, "file", fileName);

            try
            {
                using var client = new HttpClient();
                var response = await client.PostAsync($"{Constants.ServerUrl}{api}", content);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Ошибка соединения");
            }

        }

        public async Task<bool> ChangeFile(string fileID, string fileName, Stream fileStream, ProgressHandler handler)
        {
            string api = $"/api/Files/{fileID}";

            var streamContent = new ProgressStreamContent(fileStream);
            streamContent.ProgressChanged += (bytesRead) => { handler(bytesRead); };
            // Данные на отправление
            var content = new MultipartFormDataContent();
            content.Add(streamContent, "file", fileName);

            using var client = new HttpClient();
            var response = await client.PutAsync($"{Constants.ServerUrl}{api}", content);
            return response.IsSuccessStatusCode;
        }

        public async void DownloadFile(string fileID, string filePath, ProgressChangedHandler? handler = null)
        {
            string api = $"/api/Files/download/{fileID}";

            HttpClientDownloadWithProgress httpClient = new HttpClientDownloadWithProgress($"{Constants.ServerUrl}{api}", filePath);
            if (handler != null)
            {
                httpClient.ProgressChanged += handler;
            }

            await httpClient.StartDownload();
        }

        public async Task<bool> DeleteFile(string fileID)
        {
            string api = $"/api/Files/{fileID}";

            var response = await _client.DeleteAsync($"{Constants.ServerUrl}{api}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Folder> AddFolder(string name, string parentFolderID)
        {
            string api = $"/api/Folders?name={name}&parentFolderId={parentFolderID}";

            var response = await _client.PostAsync($"{Constants.ServerUrl}{api}", null);
            string responseMessage = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Folder>(responseMessage);
        }

        public async Task<bool> ChangeFolder(string folderID, string folderName)
        {
            string api = $"/api/Folders/{folderID}?name={folderName}";

            var response = await _client.PutAsync($"{Constants.ServerUrl}{api}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteFolder(string folderID)
        {
            string api = $"/api/Folders/{folderID}";

            var response = await _client.DeleteAsync($"{Constants.ServerUrl}{api}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PublicLink> LinkExists(string contentID, bool isFile)
        {
            string api = $"/public?contentID={contentID}&isFile={isFile}";

            var response = await _client.GetAsync($"{Constants.ServerUrl}{api}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseMessage = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PublicLink>(responseMessage);
        }

        public async Task<bool> DeleteLink(string contentID, bool isFile)
        {
            string api = $"/public?contentID={contentID}&isFile={isFile}";

            var response = await _client.DeleteAsync($"{Constants.ServerUrl}{api}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PublicLink> CreateLink(string contentID, bool isFile, string? downloadCount = null)
        {
            downloadCount ??= "-1";
            string api = $"/public?contentID={contentID}&isFile={isFile}&downloadCount={downloadCount}";

            var response = await _client.PostAsync($"{Constants.ServerUrl}{api}", null);
            string responseMessage = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PublicLink>(responseMessage);
        }
    }
}
