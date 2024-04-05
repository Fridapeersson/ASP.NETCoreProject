using ProjectASPNET.ViewModels.Components;

namespace ProjectASPNET.ViewModels.Home;

public class ShowcaseViewModel
{
    public string? Id { get; set; }
    public ImageViewModel Image { get; set; } = new ImageViewModel();
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public LinkViewModel Link { get; set; } = new LinkViewModel();
    public string? BrandsText { get; set; }
    public List<ImageViewModel>? BrandImages { get; set; }
}
