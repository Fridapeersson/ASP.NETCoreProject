using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Auth;

public class SignInModel
{
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [Required(ErrorMessage = "Invalid email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;



    [Display(Name = "Password", Prompt = "Enter Password")]
    [Required(ErrorMessage = "Invalid password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;


    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}
