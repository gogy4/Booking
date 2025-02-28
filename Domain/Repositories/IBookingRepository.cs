using Domain.Interfaces;
using BookingEntity = Domain.Entities.Booking;


namespace Infrastructure.Interfaces;

public interface IBookingRepository : IRepository<BookingEntity>
{
    Task<BookingEntity?> GetByIdAsync(Guid id); 
    Task UpdateAsync(IEntity booking);
    Task DeleteBooking(IEntity entity, Guid bookingId);
    Task AddAsync(BookingEntity booking);
    
    Task<List<BookingEntity>> GetAllAsync(DateTime startDate = default);
    
}