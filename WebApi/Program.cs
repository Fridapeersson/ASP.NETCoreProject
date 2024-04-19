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

builder.Services.AddScoped<SavedCoursesRepository>();


builder.Services.AddScoped<ContactRepository>();
builder.Services.AddScoped<ContactService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterSwagger();

//Rgistrera Jwt
builder.Services.RegisterJwt(builder.Configuration);


var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Silicon Web Api v1"));


app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
