namespace ProjectASPNET.ViewModels.Account;

public class ProfileInfoViewModel
{
    public string? ProfileImgUrl { get; set; } 


    public string? FirstName { get; set; } = null!;
    
    public string? LastName { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public bool IsExternalAccount { get; set; }
}
