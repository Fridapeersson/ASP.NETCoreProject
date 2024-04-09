using ProjectASPNET.ViewModels.Components;

namespace ProjectASPNET.ViewModels.Home;

public class WorkToolsViewModel
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string? Description { get; set; } 

    public HashSet<FeaturesBoxViewModel> WorkToolBox { get; set; } = [];
}
