
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using BookingEntity = Domain.Entities.Booking;

namespace Infrastructure.Repositories;

public class BookingRepository(AppDbContext context) : IBookingRepository
{
    public async Task<BookingEntity?> GetByIdAsync(Guid id)
    {
        return await context.Bookings.FindAsync(id);
    }
    
    public async Task UpdateAsync(IEntity booking)
    {
        context.Bookings.Update(booking as BookingEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteBooking(IEntity entity, Guid bookingId)
    {
        var booking = await GetByIdAsync(bookingId);
        context.Bookings.Remove(booking);
        await context.SaveChangesAsync();
    }

    public async Task AddAsync(BookingEntity booking)
    {
        await context.Bookings.AddAsync(booking);
        await context.SaveChangesAsync();
    }

    public async Task<List<BookingEntity>> GetAllAsync(DateTime startDate = default)
    {
        if (startDate == default) startDate = DateTime.Today;
        return await context.Bookings.Where(x=>x.StartDate >= startDate || x.StartDate == default).ToListAsync();
    }

    public async Task<List<BookingEntity>> GetBookingsByDate(DateTime startDate, DateTime endDate)
    {
        return await context.Bookings.Where(b=>b.StartDate >= startDate && b.EndDate <= endDate).ToListAsync();
    }
}