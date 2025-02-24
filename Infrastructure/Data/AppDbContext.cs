using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Customer?> Customers { get; set; }
    public DbSet<Booking?> Bookings { get; set; }
    public DbSet<Room?> Rooms { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
}