using Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Auth;

public class SignUpModel
{
    [Display(Name = "First name", Prompt = "Enter your first name")]
    [Required(ErrorMessage = "Invalid first name")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;



    [Display(Name = "Last name", Prompt = "Enter your last name")]
    [Required(ErrorMessage = "Invalid last name")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;



    [Display(Name = "Email", Prompt = "Enter your email address")]
    [Required(ErrorMessage = "Invalid email")]
    [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;



    [Display(Name = "Password", Prompt = "Enter Password")]
    [Required(ErrorMessage = "Invalid password")]
    [RegularExpression("^(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()-+=])[A-Za-z\\d!@#$%^&*()-+=]{8,}$")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;



    [Display(Name = "Confirm password", Prompt = "Enter password")]
    [Required(ErrorMessage = "Invalid password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Password doesn't match")]
    public string ConfirmPassword { get; set; } = null!;


    [Display(Name = "Terms & Conditions")]
    [RequiredCheckbox(ErrorMessage = "You must accept the terms & conditions")]
    public bool TermsAndConditions { get; set; }
}
