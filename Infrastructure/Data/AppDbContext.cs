using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BookingEntity = Domain.Entities.Booking;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Customer?> Customers { get; set; }
    public DbSet<BookingEntity?> Bookings { get; set; }
    public DbSet<Room?> Rooms { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
}