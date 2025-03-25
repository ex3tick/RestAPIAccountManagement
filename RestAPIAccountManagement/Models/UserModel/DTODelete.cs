using System.Text.Json.Serialization;

namespace RestAPIAccountManagement.Models.UserModel;

public class DTODelete
{
    [JsonPropertyName("userModel")]
    public UserModel User { get; set; }
   
    [JsonPropertyName("currentUser")]
    public UserModel CurrentUser { get; set; }
}