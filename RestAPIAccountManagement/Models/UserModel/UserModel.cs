using System.ComponentModel.DataAnnotations;

namespace RestAPIAccountManagement.Models.UserModel;

public class UserModel
{
    
    public int Id { get; set; } 
    
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "The Name field cannot contain special characters.")]
    public string FirstName { get; set; }
    
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "The Name field cannot contain special characters.")]
    public string LastName { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "The Name field cannot contain special characters.")]
    public string Username { get; set; } 
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 8 and 15 characters and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
    
    public string PasswordHash { get; set; } 
    [Required]
    
    public string Role { get; set; } 
    
    public string? Token { get; set; }
    
    public string Salt { get; set; }
}