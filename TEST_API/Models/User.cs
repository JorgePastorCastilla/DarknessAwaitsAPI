using System.Text.Json.Serialization;

namespace DarknessAwaits_API.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        [JsonIgnore]
        public string password { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}
