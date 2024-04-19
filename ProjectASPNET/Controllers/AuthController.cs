using Infrastructure.Entities;
using Infrastructure.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectASPNET.ViewModels.Auth;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace ProjectASPNET.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public AuthController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IConfiguration configuration, HttpClient http, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _http = http;
            _config = config;
        }

        #region Sign Up
        [HttpGet]
        [Route("/signup")]
        public IActionResult SignUp()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Details", "Account");
            }
            return View();
        }

        [HttpPost]
        [Route("/signup")]
        public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
        {
            var defaultRole = "User";

            if (ModelState.IsValid)
            {
                if(!await _userManager.Users.AnyAsync())
                {
                    defaultRole = "Admin";
                }
                var exists = await _userManager.Users.AnyAsync(x => x.Email == viewModel.Form.Email);
                if (exists)
                {
                    ModelState.AddModelError("AlreadyExists", "User with the same email address already exists");
                    ViewData["ErrorMessage"] = "User with the same email address already exists";
                    return View(viewModel);
                }

                var userEntity = new UserEntity
                {
                    FirstName = viewModel.Form.FirstName,
                    LastName = viewModel.Form.LastName,
                    Email = viewModel.Form.Email,
                    UserName = viewModel.Form.Email 
                };

                var result = await _userManager.CreateAsync(userEntity, viewModel.Form.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userEntity, defaultRole);
                    TempData["SuccessMessage"] = "Signup completed successfully";
                    return RedirectToAction("SignIn", "Auth");
                }
            }
            ModelState.AddModelError("IncorrectValues", "Incorrect Values");
            ViewData["ErrorMessage"] = "Incorrect Values";
            return View();
        }
        #endregion


        #region Sign In
        [HttpGet]
        [Route("/signin")]
        public IActionResult SignIn(string returnUrl)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Details", "Account");
            }

            ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/"); 
            return View();
        }


        [HttpPost]
        [Route("/signin")]
        public async Task<IActionResult> SignIn(SignInModel form)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, form.RememberMe, false);
                if (result.Succeeded)
                {
                    var content = new StringContent(JsonConvert.SerializeObject(form), Encoding.UTF8, "application/json");
                    var response = await _http.PostAsync($"https://localhost:7107/api/Auth/token?key={_config["ApiKey:Secret"]}", content);
                    if(response.IsSuccessStatusCode)
                    {
                        var token = await response.Content.ReadAsStringAsync();
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, //ska gå över https
                            Expires = DateTime.UtcNow.AddDays(1)
                        };


                        Response.Cookies.Append("AccessToken", token, cookieOptions);
                    }
                    
                    return RedirectToAction("Details", "Account");
                }
            }
            ModelState.AddModelError("IncorrectValues", "Incorrect email or password");
            ViewData["ErrorMessage"] = "Incorrect email or password";
            return View();
        }
        #endregion


        #region Sign Out
        [HttpGet]
        [Route("/signout")]
        public new async Task<IActionResult> SignOut()
        {
            Response.Cookies.Delete("AccessToken");

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion


        #region External Account - Facebook
        [HttpGet]
        public IActionResult Facebook()
        {
            try
            {
                var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("FacebookCallback"));
                return new ChallengeResult("Facebook", authProps);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FacebookCallback()
        {
            try
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info != null)
                {
                    var userEntity = new UserEntity
                    {
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!, 
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email!),
                        UserName = info.Principal.FindFirstValue(ClaimTypes.Email!),
                        IsExternalAccount = true,
                    };

                    var user = await _userManager.FindByEmailAsync(userEntity.Email!);
                    if (user == null)
                    {
                        var result = await _userManager.CreateAsync(userEntity);
                        if (result.Succeeded)
                        {
                            user = await _userManager.FindByEmailAsync(userEntity.Email!);
                        }
                    }

                    if(user != null)
                    {
                        if(user.FirstName != userEntity.FirstName || user.LastName != userEntity.LastName || user.Email != userEntity.Email || user.UserName != userEntity.UserName)
                        {
                            user.FirstName = userEntity.FirstName;
                            user.LastName = userEntity.LastName;
                            user.Email = userEntity.Email;
                            user.UserName = userEntity.UserName;
                            //user.IsExternalAccount = true;

                            await _userManager.UpdateAsync(user);
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false);

                        if(!HttpContext.User.Identity!.IsAuthenticated)
                        {
                            ModelState.AddModelError("InvalidFacebookAuthentication", "Failed to authenticate with facebook");
                            ViewData["ErrorMessage"] = "Failed to authenticate with facebook";
                            return RedirectToAction("SignIn", "Auth");
                        }
                    }

                }
                return RedirectToAction("Details", "Account");
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null!; 
        }
        #endregion


        [HttpGet]
        public IActionResult Google()
        {
            try
            {
                var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("GoogleCallback"));
                return new ChallengeResult("Google", authProps);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return View();
        }

        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info != null)
                {
                    var userEntity = new UserEntity
                    {
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!, 
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email!),
                        UserName = info.Principal.FindFirstValue(ClaimTypes.Email!),
                        IsExternalAccount = true,
                    };

                    var user = await _userManager.FindByEmailAsync(userEntity.Email!);
                    if (user == null)
                    {
                        var result = await _userManager.CreateAsync(userEntity);
                        if (result.Succeeded)
                        {
                            user = await _userManager.FindByEmailAsync(userEntity.Email!);
                        }
                    }

                    if (user != null)
                    {
                        if (user.FirstName != userEntity.FirstName || user.LastName != userEntity.LastName || user.Email != userEntity.Email || user.UserName != userEntity.UserName)
                        {
                            user.FirstName = userEntity.FirstName;
                            user.LastName = userEntity.LastName;
                            user.Email = userEntity.Email;
                            user.UserName = userEntity.UserName;
                            //user.IsExternalAccount = true;

                            await _userManager.UpdateAsync(user);
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false);

                        if (!HttpContext.User.Identity!.IsAuthenticated)
                        {
                            ModelState.AddModelError("InvalidGoogleAuthentication", "Failed to authenticate with google");
                            ViewData["ErrorMessage"] = "Failed to authenticate with google";
                            return RedirectToAction("SignIn", "Auth");
                        }
                    }
                }
                return RedirectToAction("Details", "Account");
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null!;
        }
    }
}
