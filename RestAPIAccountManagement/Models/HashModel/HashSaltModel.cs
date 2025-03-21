using System.ComponentModel.DataAnnotations;

namespace RestAPIAccountManagement.Models.HashModel;

public class HashSaltModel
{

    public  string Password { get; set; }
 
    public string? Salt { get; set; }
}