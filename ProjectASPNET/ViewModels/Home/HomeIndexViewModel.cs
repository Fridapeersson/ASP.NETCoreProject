namespace ProjectASPNET.ViewModels.Home;

public class HomeIndexViewModel
{
    public string Title { get; set; } = "";
    public ShowcaseViewModel Showcase { get; set; } = null!;
    public FeaturesViewModel Features { get; set; } = null!;
    public SwitchLightDarkModeViewModel SwitchLight {  get; set; } = null!;
    public ManageYourWorkViewModel ManageYourWork { get; set; } = null!;
    public DownloadOurAppViewModel DownloadOurApp { get; set; } = null!;
    public WorkToolsViewModel WorkTools { get; set; } = null!;

    public SubscribeViewModel Subscriber { get; set; } = null!;
}
