using ProjectASPNET.ViewModels.Courses;

namespace ProjectASPNET.ViewModels.Account;

public class AccountDetailsViewModel
{
    public BasicInfoViewModel? BasicInfo { get; set; }
    public AddressInfoViewModel? AddressInfo { get; set; }
    public ProfileInfoViewModel? ProfileInfo { get; set; }

    public bool IsExternalAccount { get; set; }

    public List<CourseViewModel>? SavedCourses { get; set; }
}





