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

    public async Task<List<Booking?>> GetByStatusAsync(BookingStatus status = BookingStatus.All)
    {
        if (status == BookingStatus.All) return await context.Bookings.ToListAsync();
        return await context.Bookings.Where(x=> x.Status == status).ToListAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        context.Bookings.Update(booking);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var booking = await GetByIdAsync(id);
        if (booking is null) return;
        context.Bookings.Remove(booking);
        await context.SaveChangesAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        await context.Bookings.AddAsync(booking);
        await context.SaveChangesAsync();
    }
}