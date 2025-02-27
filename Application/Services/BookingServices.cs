using Domain.Entities;
using BookingEntity = Domain.Entities.Booking;
using Infrastructure.Interfaces;

namespace Application.Services;

public class BookingServices(IBookingRepository bookingRepository)
{
    public async Task<BookingEntity> CreateBooking(Guid customerId, DateTime startDate, DateTime endDate, Guid roomId)
    {
        if (startDate >= endDate) throw new ArgumentException("Start date cannot be earlier than end date");
        var booking = new BookingEntity(customerId, startDate, endDate, roomId);
        await bookingRepository.AddAsync(booking);
        return booking;
    }

    public async Task<List<BookingEntity>> GetBookings(DateTime startDate = default)
    {
        var bookings = await bookingRepository.GetAllAsync(startDate);
        if (bookings is null) throw new NullReferenceException("Booking not found");
        return bookings;
    }

    public async Task UpdateBooking(BookingEntity booking)
    {
        await bookingRepository.UpdateAsync(booking);
    }

    public async Task DeleteBooking(BookingEntity booking)
    {
        await bookingRepository.DeleteBooking(booking, booking.Id);
    }

    public async Task<BookingEntity?> GetById(Guid id)
    {
        return await bookingRepository.GetByIdAsync(id);
    }
}