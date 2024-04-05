using Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Account;

public class DeleteAccountModel
{
    [Required]
    [Display(Name = "Delete confirm")]
    [RequiredCheckbox(ErrorMessage = "You must check the box if you want to delte your account")]
    public bool ConfirmDeleteAccount { get; set; } = false;
}
