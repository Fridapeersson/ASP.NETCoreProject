using Infrastructure.Models.Auth;

namespace ProjectASPNET.ViewModels.Auth;

public class SignInViewModel
{
    public string Title { get; set; } = "Sign In";
    public string? ErrrorMessage { get; set; }
    public SignInModel Form { get; set; } = new();

}

    //[Display(Name = "Email", Prompt = "Enter your email address")]
    //[Required(ErrorMessage = "Invalid email")]
    //[DataType(DataType.EmailAddress)]
    //public string Email { get; set; } = null!;



    //[Display(Name = "Password", Prompt = "Enter Password")]
    //[Required(ErrorMessage = "Invalid password")]
    //[DataType(DataType.Password)]
    //public string Password { get; set; } = null!;

    //public bool RememberMe { get; set; }