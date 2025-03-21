using System.ComponentModel.DataAnnotations;

namespace RestAPIAccountManagement.Models.HashModel;

public class HashSaltModel
{
    [Required]
    public  string Password { get; set; }
    [Required]
    public string? Salt { get; set; }
}