using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;


namespace Infrastructure.Services;

public class AccountService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly DataContext _context;
    private readonly IConfiguration _config;

    public AccountService(UserManager<UserEntity> userManager, DataContext context, IConfiguration config)
    {
        _userManager = userManager;
        _context = context;
        _config = config;
    }

    /// <summary>
    ///     Uploads a user's profile image async
    /// </summary>
    /// <param name="user">the user who is uploading the image</param>
    /// <param name="file">the file to be uploaded</param>
    /// <returns>true if uploaded successfully, else false</returns>
    public async Task<bool> UploadUserProfileImageAsync(ClaimsPrincipal user, IFormFile file)
    {
        try
        {
            if(user != null && file != null && file.Length != 0)
            {
                var userEntity = await _userManager.GetUserAsync(user);
                if (userEntity != null)
                {
                    var fileName = $"p_{userEntity.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), _config["FileUploadPath"]!, fileName);

                    using var fileStream = new FileStream(filePath, FileMode.Create);

                    await file.CopyToAsync(fileStream);

                    userEntity.ProfileImgUrl = fileName;
                    _context.Update(userEntity);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

}
