
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class SubscribeModel
{
    [Required(ErrorMessage = "You have to enter a valid email")]
    [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$", ErrorMessage = "Invalid email address")]
    [EmailAddress]
    public string Email { get; set; } = null!;


    [Display(Name = "Daily Newsletter")]
    public bool DailyNewsletter { get; set; } = false;


    [Display(Name = "Event Updates")]
    public bool EventUpdates { get; set; } = false;


    [Display(Name = "Advertising Updates")]
    public bool AdvertisingUpdates { get; set; } = false;


    [Display(Name = "Startups Weekly")]
    public bool StartupsWeekly { get; set; } = false;


    [Display(Name = "Week In Review")]
    public bool WeekInReview { get; set; } = false;


    [Display(Name = "Podcasts")]
    public bool Podcasts { get; set; } = false;
}






//[Required(ErrorMessage = "You have to enter a name")]
//[Display(Name = "Full name", Prompt = "Enter your full name")]
//[DataType(DataType.Text)]
//public string Name { get; set; } = null!;



//[Required(ErrorMessage = "You have to enter a valid email")]
//[Display(Name = "Full name", Prompt = "Enter your email address")]
//[RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$")]
//[DataType(DataType.EmailAddress)]
//public string Email { get; set; } = null!;



//[Display(Name = "Services", Prompt = "Choose the service you are interested in")]
//[DataType(DataType.Text)]
//public string? Service { get; set; }



//[Required(ErrorMessage = "You have to enter a message")]
//[Display(Name = "Message", Prompt = "Enter your message here...")]
//[DataType(DataType.MultilineText)]
//public string Message { get; set; } = null!;