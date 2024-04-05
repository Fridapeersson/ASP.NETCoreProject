using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class SubscribersEntity
{
    public int Id { get; set; }


    [Required]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    //[EmailAddress]
    public string Email { get; set; } = null!;

    public bool DailyNewsletter { get; set; } = false;
    public bool EventUpdates { get; set; } = false;
    public bool AdvertisingUpdates { get; set; } = false;
    public bool StartupsWeekly { get; set; } = false;
    public bool WeekInReview { get; set; } = false;
    public bool Podcasts { get; set; } = false;

    public static implicit operator SubscribersEntity(SubscribeModel model)
    {
        return new SubscribersEntity
        {
            Email = model.Email,
            DailyNewsletter = model.DailyNewsletter,
            EventUpdates = model.EventUpdates,
            AdvertisingUpdates = model.AdvertisingUpdates,
            StartupsWeekly = model.StartupsWeekly,
            WeekInReview = model.WeekInReview,
            Podcasts = model.Podcasts,
        };
    }
}
