namespace ProjectASPNET.ViewModels.Home;

public class HomeIndexViewModel
{
    public string Title { get; set; } = "";
    public ShowcaseViewModel Showcase { get; set; } = null!;
    public FeaturesViewModel Features { get; set; } = null!;

    public SubscribeViewModel Subscriber { get; set; } = null!;
}
