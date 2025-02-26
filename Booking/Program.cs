using Application.Services;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Booking API",
        Version = "v1",
        Description = "API для управления бронированием номеров"
    });
});

var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();