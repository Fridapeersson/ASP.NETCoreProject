using Infrastructure.Models.Auth;

namespace ProjectASPNET.ViewModels.Auth;

public class SignUpViewModel
{
    public string Title { get; set; } = "Sign Up";
    public string? ErrrorMessage { get; set; }
    public SignUpModel Form { get; set; } = new();
}
