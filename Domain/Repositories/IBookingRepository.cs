using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Infrastructure.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<Booking?> GetByIdAsync(Guid id); 
    Task UpdateAsync(IEntity booking);
    Task DeleteBooking(IEntity entity, Guid bookingId);
    Task AddAsync(Booking booking);
    
    Task<List<Booking>> GetAllAsync(DateTime startDate = default);

    Task<List<Booking>> GetBookingsByDate(DateTime startDate, DateTime endDate);
    
}