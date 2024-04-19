
using Infrastructure.Models.Dtos.Contact;

namespace ProjectASPNET.ViewModels.Contact;

public class ContactFormViewModel
{
    public string Title { get; set; } = "Get In Contact With Us";
    public string? ErrorMessage { get; set; }
    public ContactDto Contact { get; set; } = new ContactDto();
}
