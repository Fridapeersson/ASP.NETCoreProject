using Azure;
using Infrastructure.Entities;
using Infrastructure.Models.Account;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectASPNET.ViewModels.Account;
using ProjectASPNET.ViewModels.Courses;
using System.Diagnostics;
using System.Security.Claims;

namespace ProjectASPNET.Controllers;


[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly AddressService _addressService;
    private readonly CoursesService _coursesService;
    private readonly AccountService _accountService;

    private readonly HttpClient _http;


    public AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, AddressService addressService, CoursesService coursesService, HttpClient http, AccountService accountService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _addressService = addressService;
        _coursesService = coursesService;
        _http = http;
        _accountService = accountService;
    }


    #region [HttpGET] Details
    [HttpGet]
    [Route("/account/details")]
    public async Task<IActionResult> Details()
    {
        if(ModelState.IsValid)
        {
            try
            {
                var claims = HttpContext.User.Identities.FirstOrDefault();
                ViewBag.ActiveMenu = "Details";
                var viewModel = await PopulateAccountViewModel();

                return View(viewModel);
            }
            catch(Exception ex) { Debug.WriteLine(ex); }
        }
        return null!;
    }
    #endregion


    #region [HttPOST] Details
    [HttpPost]
    [Route("/account/details")]
    public async Task<IActionResult> Details(AccountDetailsViewModel viewModel)
    {
        try
        {
            
            if(ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(User);
                if (viewModel.BasicInfo != null)
                {
                    await UpdateBasicInfoAsync(viewModel.BasicInfo);
                }
                if (viewModel.AddressInfo != null)
                {
                    await UpdateAddressInfo(viewModel.AddressInfo);
                }
                ViewData["Message"] = "Your account details have been updated successfully.";
                await PopulateAccountViewModel();
            }
        }
        catch(Exception ex) { Debug.WriteLine(ex); }




        viewModel.BasicInfo ??= await PopulateBasicInfoAsync();
        viewModel.AddressInfo ??= await PopulateAddressInfoAsync();
        viewModel.ProfileInfo ??= await PopulateProfileInfoAsync();
        ViewBag.ActiveMenu = "Details";

        return View(viewModel);
    }
    #endregion


    #region [HttpGET] SavedCourses
    [HttpGet]
    [Route("/account/savedCourses")]
    public async Task<IActionResult> SavedCourses()
    {
        if(ModelState.IsValid)
        {
            try
            {
                ViewBag.ActiveMenu = "SavedCourses";

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var profileInfo = await PopulateProfileInfoAsync();

                var allCourses = await _coursesService.GetAllAsync();
                var savedCourses = await _coursesService.GetAllSavedCourses(User);
                var savedCourseIds = savedCourses.Select(x => x.CourseId);
                var savedCoursesViewModels = new List<CourseViewModel>();

                //loopa igenom kurserna och skapa objekt för de sparade kurserna
                foreach (var course in allCourses)
                {
                    if (savedCourseIds.Contains(course.Id))
                    {
                        var courseViewModel = new CourseViewModel
                        {
                            Id = course.Id,
                            Title = course.Title,
                            Price = course.Price,
                            DiscountPrice = course.DiscountPrice,
                            HoursToComplete = course.HoursToComplete,
                            LikesInNumbers = course.LikesInNumbers,
                            LikesInPercent = course.LikesinPercent,
                            IsBestSeller = course.IsBestSeller,
                            BackgroundImageName = course.BackgroundImageName!,
                            Author = new AuthorViewModel
                            {
                                Id = course.Author.Id,
                                AuthorTitle = course.Author.AuthorTitle,
                                AuthorName = course.Author.AuthorName,
                                AuthorDescription = course.Author.AuthorDescription,
                                AuthorImageUrl = course.Author.AuthorImageUrl,
                                FacebookSubs = course.Author.FacebookSubs,
                                YoutubeSubs = course.Author.YoutubeSubs
                            }
                        };
                        // ändrar så kursen är true
                        courseViewModel.IsSaved = true;

                        savedCoursesViewModels.Add(courseViewModel);
                    }
                }
                var viewModel = new AccountDetailsViewModel
                {
                    ProfileInfo = profileInfo,
                    SavedCourses = savedCoursesViewModels
                };
                return View(viewModel);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
        return View();
    }
    #endregion


    [HttpPost]
    public async Task<IActionResult> SaveCourseToProfile(int courseId)
    {
        if(ModelState.IsValid)
        {
            try
            {
                //hämtar användarens id
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    var result = await _coursesService.SaveCourseToProfile(userId, courseId);

                    if (result)
                    {
                        return RedirectToAction("SavedCourses", "Account");
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }

        }
         return RedirectToAction("SavedCourses", "Account"); 
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCourseFromProfile(int courseId)
    {
        if(ModelState.IsValid)
        {
            try
            {
                //hämtar användarens id
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    var result = await _coursesService.RemoveCourseFromProfileAsync(userId, courseId);

                    if (result)
                    {
                        return RedirectToAction("SavedCourses", "Account");
                    }
                }
            }
            catch(Exception ex) { Debug.WriteLine(ex); }
        }

        return RedirectToAction("SavedCourses", "Account");
    }

    public async Task<IActionResult> RemoveAllSavedCourses()
    {
        if(ModelState.IsValid)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    var result = await _coursesService.RemoveAllSavedCoursesAsync(userId);
                    if (result)
                    {
                        return RedirectToAction("SavedCourses", "Account");
                    }
                    else
                    {
                        return RedirectToAction("SavedCourses", "Account");
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
        return null!;
    }

    #region GET - Security
    [HttpGet]
    [Route("/account/security")]
    public async Task<IActionResult> Security()
    {

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId!);
            if (user != null)
            {
                var profileInfo = new ProfileInfoViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    ProfileImgUrl = user.ProfileImgUrl,
                    IsExternalAccount = user.IsExternalAccount,
                };
                var viewModel = new SecurityViewModel
                {
                    ProfileInfo = profileInfo,
                };
                ViewBag.ActiveMenu = "Security";
                return View(viewModel);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }


        return View(new SecurityViewModel());
    }
    #endregion

#region
[HttpPost]
    public async Task<IActionResult> Security(SecurityViewModel viewModel)
    {
        try
        {
            ViewBag.ActiveMenu = "Security";

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if(user != null)
                {
                    viewModel.ProfileInfo = await PopulateProfileInfoAsync();
                    var checkPassword = await _userManager.CheckPasswordAsync(user, viewModel.Form!.CurrentPassword!);
                    if(!string.IsNullOrWhiteSpace(viewModel.Form.CurrentPassword))
                    {
                        var result = await _userManager.ChangePasswordAsync(user, viewModel.Form.CurrentPassword, viewModel.Form.NewPassword!);
                        if(result.Succeeded)
                        {
                            ViewBag.ActiveMenu = "Security";
                            ViewData["Message"] = "Password changed successfully.";
                            return View(viewModel);
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Something went wrong";
                        }
                    }
                }
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var profileInfo = await PopulateProfileInfoAsync();
                if (profileInfo != null)
                {
                    var securityViewModel = new SecurityViewModel
                    {
                        ProfileInfo = profileInfo,
                    };
                    ViewData["ErrorMessage"] = "Incorrect values";
                    return View(securityViewModel);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        //ViewBag.ActiveMenu = "Security";
        return View(viewModel);
    }
    #endregion

    [HttpPost]
    public async Task<IActionResult> DeleteAccount(SecurityViewModel viewModel)
    {
        try
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _userManager.DeleteAsync(user);
                    if( result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewData["Error"] = "Something went wrong";
                    }
                }
            }
            else
            {
                viewModel.ProfileInfo = await PopulateProfileInfoAsync();
                ViewData["Error"] = "You must check the box if you want to delete your account.";
                ViewBag.ActiveMenu = "Security";
                return View("Security", viewModel);

            }
            return RedirectToAction("Security", "Account");
 
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }


    private async Task<AccountDetailsViewModel> PopulateAccountViewModel()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var viewModel = new AccountDetailsViewModel
                {
                    IsExternalAccount = user.IsExternalAccount,
                    BasicInfo = await PopulateBasicInfoAsync(),
                    AddressInfo = await PopulateAddressInfoAsync(),
                    ProfileInfo = await PopulateProfileInfoAsync(),
                };
                return viewModel;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }

        return new AccountDetailsViewModel
        {
            BasicInfo = new BasicInfoViewModel { },
            AddressInfo = new AddressInfoViewModel { },
            ProfileInfo = new ProfileInfoViewModel { },
        };
    }


    /// <summary>
    ///     Populates a profileInfoViewModel with profile information from database async
    /// </summary>
    /// <returns>The populated profile information, else null</returns>
    private async Task<ProfileInfoViewModel> PopulateProfileInfoAsync()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            //if(user == null)
            //{
            //    return null!;
            //}
            var profileInfo = new ProfileInfoViewModel
            {
                FirstName = user!.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImgUrl = user.ProfileImgUrl,
            };
            return profileInfo;
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }


    /// <summary>
    ///      Populates a BasicInfoViewModel with basic information from database async
    /// </summary>
    /// <returns>The populated basic information, else null</returns>
    private async Task<BasicInfoViewModel> PopulateBasicInfoAsync()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            var basicInfoViewModel = new BasicInfoViewModel
            {
                FirstName = user!.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Biography = user.Biography,
            }; 
            return basicInfoViewModel;
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    /// <summary>
    ///     Updates the basic information of a user async
    /// </summary>
    /// <param name="basicInfo">The basic information to update</param>
    private async Task UpdateBasicInfoAsync(BasicInfoViewModel basicInfo)
    {
        if (basicInfo.FirstName != null && basicInfo.LastName != null && basicInfo.Email != null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.FirstName = basicInfo.FirstName;
                user.LastName = basicInfo.LastName;
                user.Email = basicInfo.Email;
                user.PhoneNumber = basicInfo.PhoneNumber;
                user.Biography = basicInfo.Biography;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("IncorrectValues", "Something went wrong, unable to update basic information");
                    ViewData["ErrorMessage"] = "Something went wrong, unable to update basic information";
                }
            }
        }
    }

    /// <summary>
    ///     Populates a AddressInfoViewModel with address information from database async
    /// </summary>
    /// <returns>The populated address information, else null</returns>
    private async Task<AddressInfoViewModel> PopulateAddressInfoAsync()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var address = await _addressService.GetAddressAsync(user.Id);

                if(address == null)
                {
                    return new AddressInfoViewModel();
                }
                var addressInfo = new AddressInfoViewModel
                {
                    AddressLine_1 = address.AddressLine_1,
                    AddressLine_2 = address.AddressLine_2,
                    PostalCode = address.PostalCode,
                    City = address.City
                };
                return addressInfo;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return new AddressInfoViewModel();
    }

    /// <summary>
    ///     Updates the address information of a user async
    /// </summary>
    /// <param name="addressInfo">The address information to update</param>
    private async Task UpdateAddressInfo(AddressInfoViewModel addressInfo)
    {
        try
        {
            if (addressInfo.AddressLine_1 != null && addressInfo.PostalCode != null && addressInfo.City != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var address = await _addressService.GetAddressAsync(user.Id);
                    if (address != null)
                    {
                        address.AddressLine_1 = addressInfo.AddressLine_1;
                        address.AddressLine_2 = addressInfo.AddressLine_2;
                        address.PostalCode = addressInfo.PostalCode;
                        address.City = addressInfo.City;

                        var result = await _addressService.UpdateAddressAsync(address);
                        if (!result)
                        {
                            ModelState.AddModelError("IncorrectValues", "Something went wrong, unable to update address information");
                            ViewData["ErrorMessage"] = "Something went wrong, unable to update address information";
                        }
                    }
                    else
                    {
                        address = new AddressEntity
                        {
                            UserId = user.Id,
                            AddressLine_1 = addressInfo.AddressLine_1,
                            AddressLine_2 = addressInfo.AddressLine_2,
                            PostalCode = addressInfo.PostalCode,
                            City = addressInfo.City,
                        };

                        var result = await _addressService.CreateAddressAsync(address);
                        if (!result)
                        {
                            ModelState.AddModelError("IncorrectValues", "Something went wrong, unable to update address information");
                            ViewData["ErrorMessage"] = "Something went wrong, unable to update address information";
                        }
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); };
    }

    //Profile image
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await _accountService.UploadUserProfileImageAsync(User, file);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
        return RedirectToAction("Details", "Account");
    }
}