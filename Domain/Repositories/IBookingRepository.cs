using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id);
    Task<List<Booking?>> GetByStatusAsync(BookingStatus status = BookingStatus.All);
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(Guid id);
    Task AddAsync(Booking booking);
}