using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));


builder.Services.AddDefaultIdentity<UserEntity>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<UserEntity>>();
builder.Services.AddScoped<SignInManager<UserEntity>>();


// Add services to the container.
builder.Services.AddScoped<SubscribersRepository>();
builder.Services.AddScoped<SubscribersService>();

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<CoursesService>();

builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<AuthorRepository>();
//builder.Services.AddScoped<AuthorService>();

builder.Services.AddScoped<SavedCoursesRepository>();


builder.Services.AddScoped<ContactRepository>();
builder.Services.AddScoped<ContactService>();








builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterSwagger();

//Rgistrera Jwt
builder.Services.RegisterJwt(builder.Configuration);


var app = builder.Build();

//CORS : cross origin resource sharing, s� vi kan begr�nsa vilka som har tillg�ng till api't genom vilken address man kommer fr�n, man kna begr�nsa det baserat p� headers, eller metoder. Beh�ver alltid g�ra
//till�ter alla headers - kan va olika content-types, eller keys osv eller vad vi nu vill begr�nsa f�r headers
//till�t alla origin - alla, i detta fallet, f�r komma �t apiet oavset vilken address, portnummer man har
//till�t alla method - inneb�r att man till�ter (i detta fallet) alla crud-delar
app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

//kan g�ra egen cors
//builder.Services.AddCors(x =>
//{
//    x.AddPolicy("CustomOriginPolicy", policy =>
//    {
//        policy
//        .WithOrigins("http://123.4.5.6:7700") // anger vilka som kan komma �t apiet
//        .AllowAnyHeader()
//        .AllowAnyMethod();
//    });
//});
////Anv�nda egna cors
//app.UseCors("CustomOriginPolicy");




app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Silicon Web Api v1"));



app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
