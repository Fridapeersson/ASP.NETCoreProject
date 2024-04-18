using System.ComponentModel.DataAnnotations;

namespace ProjectASPNET.ViewModels.Account;

public class AddressInfoViewModel
{
    [Required(ErrorMessage = "Invalid Address")]
    [Display(Name = "Address line 1", Prompt = "Enter a addressline")]
    [DataType(DataType.Text)]
    public string AddressLine_1 { get; set; } = null!;


    [Display(Name = "Address line 2", Prompt = "Enter your second addressline")]
    [DataType(DataType.Text)]
    public string? AddressLine_2 { get; set; }


    [Required(ErrorMessage = "Invalid postal code")]
    [Display(Name = "Postal code", Prompt = "Enter your postal code")]
    [DataType(DataType.PostalCode)]
    public string PostalCode { get; set; } = null!;


    [Required(ErrorMessage = "Invalid city")]
    [Display(Name = "City", Prompt = "Enter a city")]
    [DataType(DataType.Text)]
    public string City { get; set; } = null!;
}