using Application.Services;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Подключение строки подключения к БД
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Регистрация репозиториев
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IRepository<Domain.Entities.Booking>, BookingRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRepository<Room>, RoomRepository>();

// Регистрация сервисов
builder.Services.AddScoped<BookingServices>();
builder.Services.AddScoped<CustomerServices>();
builder.Services.AddScoped<ConfirmRentalService>();
builder.Services.AddScoped<CancelRentalService>();
builder.Services.AddScoped<RoomServices>();
builder.Services.AddScoped<RentalService>();
builder.Services.AddScoped<AuthenticationService>();

// Настройка аутентификации
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

builder.Services.AddHttpContextAccessor();

// Настройка Razor Pages и Controllers
builder.Services.AddControllersWithViews()
    .ConfigureApplicationPartManager(apm =>
    {
        apm.ApplicationParts.Add(new AssemblyPart(typeof(API.Controllers.BookingController).Assembly));
    });
builder.Services.AddRazorPages();

// Регистрация инфраструктуры
builder.Services.AddInfrastructure(connectionString);

// Настройка Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Booking API",
        Version = "v1",
        Description = "API для управления бронированием номеров"
    });

    // Указываем поддержку файлов
    options.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });

    // Добавляем фильтр для обработки параметров типа IFormFile
    options.OperationFilter<SwaggerFileUploadOperationFilter>();
});


builder.Logging.AddConsole();  // Добавляем логирование

var app = builder.Build();

// Ожидаем только в Development-режиме включение Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Убираем явное указание маршрута для Swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API v1");
        c.RoutePrefix = "swagger";  // Устанавливаем маршрут для Swagger UI
    });
}

// Разрешаем доступ к статичным файлам (например, изображениям)
app.UseStaticFiles();

// Обработка папки для картинок
var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);  // Создаем папку для хранения изображений
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();  // Используем аутентификацию
app.UseAuthorization();

// Маршруты
app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
