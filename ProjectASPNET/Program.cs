using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectASPNET.Helpers.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();


builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));


builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<AddressService>();

builder.Services.AddScoped<AuthorRepository>();

builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<CategoryService>();


builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<CoursesService>();

builder.Services.AddScoped<SubscribersService>();
builder.Services.AddScoped<SubscribersRepository>();

builder.Services.AddScoped<ContactRepository>();
builder.Services.AddScoped<ContactService>();


builder.Services.AddScoped<SavedCoursesRepository>();

builder.Services.AddScoped<AccountService>();



//builder.Services.AddScoped<ContactRepository>();
//builder.Services.AddScoped<ContactService>();










//man kan köra denna eller nedanstående (AddDefaultIdentity), så slipper man ha med ".AddRoles<IdentityRole>()" denna kommer påverka databasen men inte Default versionen
//builder.Services.AddIdentity<UserEntity, IdentityRole>(x =>
//{
//    x.User.RequireUniqueEmail = true;
//    x.SignIn.RequireConfirmedAccount = false;
//    x.Password.RequiredLength = 8;

//}).AddEntityFrameworkStores<DataContext>();
builder.Services.AddDefaultIdentity<UserEntity>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = false;
    x.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();






builder.Services.ConfigureApplicationCookie(x =>
{
    x.Cookie.HttpOnly = true; //webbläsaen kan inte komma åt informationen, kan inte läsa ut cookie infon
    x.LoginPath = "/signin"; //använder man authorize måste man skriva över standard omdirigeringen
    x.AccessDeniedPath = "/denied";
    x.LogoutPath = "/signout"; 
    x.AccessDeniedPath = "/denied";

    x.Cookie.SecurePolicy = CookieSecurePolicy.Always; //alla requests man använder går via https, ska vara på alla sidor
    x.ExpireTimeSpan = TimeSpan.FromMinutes(60); // kommer loggas ut efter 60 min inaktivitet
    x.SlidingExpiration = true; // har användaren vart inaktiv i 10 min och sedan blir aktiv nollställs TimeSpanen
    
});


builder.Services.AddAuthorization(x =>
{
    //anger olika behörighetspolicys för olika användarroller
    x.AddPolicy("SuperAdmins", policy => policy.RequireRole("SuperAdmin"));
    x.AddPolicy("CIO", policy => policy.RequireRole("SuperAdmin", "CIO"));
    x.AddPolicy("Admins", policy => policy.RequireRole("Admin", "SuperAdmin", "CIO", "CEO"));
    x.AddPolicy("Managers", policy => policy.RequireRole("Admin", "SuperAdmin", "CIO", "CEO", "Manager"));
    x.AddPolicy("AuthenticatedUsers", policy => policy.RequireRole("Admin", "SuperAdmin", "CIO", "CEO", "Manager", "User"));
});


builder.Services.AddAuthentication()
    .AddGoogle(x =>
    {
        x.ClientId = "100787358579-jq7tgj796ge4d1t82ngmidk63s86hoqe.apps.googleusercontent.com";
        x.ClientSecret = "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";
       
    })
    .AddFacebook(x =>
    {
        x.AppId = "700959072110367";
        x.AppSecret = "02a8ceceb7cfc2e2240b91ed488d3224";
        x.Fields.Add("first_name");
        x.Fields.Add("last_name");
    });







var app = builder.Build();
app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}"); /*om ingen sida hittas går den till error*/
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
//Middleware
app.UseUserSessionValidation();
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = ["Admin", "User", "SuperAdmin", "CIO", "Manager"];
    foreach(var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
            //skapar bara roller som inte existerar, finns de redan kommer de inte skapas
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}



    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
