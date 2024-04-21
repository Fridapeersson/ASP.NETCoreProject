using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Account;

public class ChangePasswordModel
{
    [Display(Name = "Current password", Prompt = "********")]
    [Required(ErrorMessage = "Invalid password")]
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; } = null!;


    [Display(Name = "New password", Prompt = "********")]
    [Required(ErrorMessage = "Invalid password")]
    [RegularExpression("^(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()-+=])[A-Za-z\\d!@#$%^&*()-+=]{8,}$", ErrorMessage = "Invalid password")]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; } = null!;


    [Display(Name = "Confirm new password", Prompt = "********")]
    [Required(ErrorMessage = "Passwords doesn't match")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords doesn't match")]
    public string? ConfirmPassword { get; set; } = null!;
}
