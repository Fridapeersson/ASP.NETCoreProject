using System.ComponentModel.DataAnnotations;

namespace ProjectASPNET.ViewModels.Account;

public class BasicInfoViewModel
{
    //public string UserId { get; set; } = null!;

    [Required(ErrorMessage = "Invalid first name")]
    [DataType(DataType.Text)]
    [Display(Name = "First name", Prompt = "Enter your first name")]
    public string FirstName { get; set; } = null!;



    [Required(ErrorMessage = "Invalid last name")]
    [DataType(DataType.Text)]
    [Display(Name = "Last name", Prompt = "Enter your last name")]
    public string LastName { get; set; } = null!;



    [Required(ErrorMessage = "Invalid email")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;


    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone number", Prompt = "Enter your phone number")]
    public string? PhoneNumber { get; set; }


    [DataType(DataType.MultilineText)]
    [Display(Name = "Bio", Prompt = "Add a short bio...")]
    public string? Biography { get; set; }
}
