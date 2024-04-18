using Infrastructure.Models.Account;

namespace ProjectASPNET.ViewModels.Account;

public class SecurityViewModel
{
    public string? Title { get; set; }
    public string? ErrrorMessage { get; set; }
    public ChangePasswordModel? Form { get; set; }
    public DeleteAccountModel? DeleteAccount { get; set; }
    public ProfileInfoViewModel ProfileInfo { get; set; } = new ProfileInfoViewModel();
}
