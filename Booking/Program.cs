using Application.Services;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IRepository<Domain.Entities.Booking>, BookingRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRepository<Room>, RoomRepository>();

builder.Services.AddScoped<BookingServices>();
builder.Services.AddScoped<CustomerServices>();
builder.Services.AddScoped<ConfirmRentalService>();
builder.Services.AddScoped<CancelRentalService>();
builder.Services.AddScoped<RoomServices>();
builder.Services.AddScoped<RentalService>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
    .ConfigureApplicationPartManager(apm =>
    {
        apm.ApplicationParts.Add(new AssemblyPart(typeof(API.Controllers.BookingController).Assembly));
    });

builder.Services.AddRazorPages();
builder.Services.AddInfrastructure(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Logging.AddDebug();
builder.Logging.AddConsole();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API for managing rooms",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "support@example.com"
        }
    });
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var user = context.User;
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        if (!user.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access Denied: Only admins can access Swagger");
            return;
        }
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();

var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
