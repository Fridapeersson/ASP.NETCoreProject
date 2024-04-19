
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