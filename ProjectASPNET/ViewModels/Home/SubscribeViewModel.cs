using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectASPNET.ViewModels.Home;

public class SubscribeViewModel
{
    public string? ErrorMessage { get; set; }
    public SubscribeModel SubscribeModel { get; set; } = null!;
}


////[Required]
//////[EmailAddress]
////[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
////[Display(Name = "Email", Prompt = "Your Email")]
//public string Email { get; set; } = null!;


////[Display(Name = "Daily Newsletter")]
//public bool DailyNewsletter { get; set; } 


////[Display(Name = "Event Updates")]
//public bool EventUpdates { get; set; } 


////[Display(Name = "Advertising Updates")]
//public bool AdvertisingUpdates { get; set; } 


////[Display(Name = "Startups Weekly")]
//public bool StartupsWeekly { get; set; } 


////[Display(Name = "Week In Review")]
//public bool WeekInReview { get; set; }


////[Display(Name = "Podcasts")]
//public bool Podcasts { get; set; }
