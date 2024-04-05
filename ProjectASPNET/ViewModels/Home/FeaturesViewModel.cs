using ProjectASPNET.ViewModels.Components;

namespace ProjectASPNET.ViewModels.Home;

public class FeaturesViewModel
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public HashSet<FeaturesBoxViewModel> FeaturesBox { get; set; } = null!;
}
