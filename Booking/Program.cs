using Application.Services;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<BookingServices>();
builder.Services.AddScoped<CustomerServices>();
builder.Services.AddScoped<RoomServices>();
builder.Services.AddControllers();
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();