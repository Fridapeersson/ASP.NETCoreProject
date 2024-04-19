using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectASPNET.ViewModels.Home;

public class SubscribeViewModel
{
    public string? ErrorMessage { get; set; }
    public SubscribeModel SubscribeModel { get; set; } = null!;
}