using System.Text.Json.Serialization;

namespace RestAPIAccountManagement.Models.UserModel;

public class DTOUpdate
{
    [JsonPropertyName("userModel")]
    public UserModel ToUpdate { get; set; }
    [JsonPropertyName("currentUser")]
    public UserModel CurrentUser { get; set; }
}