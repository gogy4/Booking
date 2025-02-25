using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookingRepository(AppDbContext context) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await context.Bookings.FindAsync(id);
    }
    
    public async Task UpdateAsync(Booking booking)
    {
        context.Bookings.Update(booking);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Booking booking)
    {
        context.Bookings.Remove(booking);
        await context.SaveChangesAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        await context.Bookings.AddAsync(booking);
        await context.SaveChangesAsync();
    }

    public async Task<List<Booking>> GetAllAsync(DateTime startDate = default)
    {
        if (startDate == default) startDate = DateTime.Today;
        return await context.Bookings.Where(x=>x.StartDate >= startDate || x.StartDate == default).ToListAsync();
    }

    public async Task<List<Booking>> GetBookingsByDate(DateTime startDate, DateTime endDate)
    {
        return await context.Bookings.Where(b=>b.StartDate >= startDate && b.EndDate <= endDate).ToListAsync();
    }
}