using System.Text.Json.Serialization;

namespace Server.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public string? Email { get; set; }

        public User()
        {
            
        }
    }
}
