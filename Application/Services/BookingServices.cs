using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;

namespace Application.Services;

public class BookingServices(
    IBookingRepository bookingRepository,
    IRoomRepository roomRepository,
    ICustomerRepository customerRepository)
{
    public async Task<Booking> CreateBooking(Guid roomId, List<Guid> customerIds, DateTime startDate, DateTime endDate,
        BookingStatus status)
    {
        var room = await roomRepository.GetByIdAsync(roomId);
        if (room is null) throw new NullReferenceException("Room not found");
        var customers = await customerRepository.GetCustomersAsync(customerIds);
        if (customers is null) throw new NullReferenceException("Customers not found");
        if (startDate >= endDate) throw new ArgumentException("Start date cannot be earlier than end date");
        var booking = new Booking(roomId, customerIds, startDate, endDate, status);
        await bookingRepository.AddAsync(booking);
        return booking;
    }

    public async Task<List<Booking>> GetBookings(BookingStatus status)
    {
        var bookings = await bookingRepository.GetByStatusAsync(status);
        if (bookings is null) throw new NullReferenceException("Booking not found");
        return bookings;
    }

    public async Task UpdateBooking(Guid bookingId)
    {
        await bookingRepository.UpdateAsync(bookingId);
    }

    public async Task DeleteBooking(Guid bookingId)
    {
        await bookingRepository.DeleteAsync(bookingId);
    }

    public async Task<Booking?> GetById(Guid id)
    {
        return await bookingRepository.GetByIdAsync(id);
    }
    
    public async Task CancelRental(Guid bookingId)
    {
        await ChangeData(bookingId, booking => booking.CancelRental());
    }

    public async Task ConfirmRental(Guid bookingId)
    {
        await ChangeData(bookingId, booking => booking.ConfirmRental());
    }

    public async Task ChangeDate(Guid bookingId, DateTime newStartDate, DateTime newEndDate)
    {
        await ChangeData(bookingId, booking => booking.ChangeDate(newStartDate, newEndDate));
    }

    public async Task ChangeStartDate(Guid bookingId, DateTime newStartDate)
    {
        await ChangeData(bookingId, booking => booking.ChangeStartDate(newStartDate));
    }

    public async Task ChangeEndDate(Guid bookingId, DateTime newEndDate)
    {
        await ChangeData(bookingId, booking => booking.ChangeEndDate(newEndDate));
    }

    private async Task ChangeData(Guid bookingId, Action<Booking> changeBookingData)
    {
        var booking = await bookingRepository.GetByIdAsync(bookingId);
        if (booking is null) throw new KeyNotFoundException("Booking not found");
        changeBookingData(booking);
        await bookingRepository.UpdateAsync(bookingId);
    }
}