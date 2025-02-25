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

    public async Task<List<Booking>> GetBookings(BookingStatus status = BookingStatus.All)
    {
        var bookings = await bookingRepository.GetByStatusAsync(status);
        if (bookings is null) throw new NullReferenceException("Booking not found");
        return bookings;
    }

    public async Task UpdateBooking(Booking booking)
    {
        await bookingRepository.UpdateAsync(booking);
    }

    public async Task<List<Booking>> GetBookingsByDate(DateTime startDate, DateTime endDate)
    {
        return await bookingRepository.GetBookingsByDate(startDate, endDate);
    }

    public async Task DeleteBooking(Booking booking)
    {
        await bookingRepository.DeleteAsync(booking);
    }

    public async Task<Booking?> GetById(Guid id)
    {
        return await bookingRepository.GetByIdAsync(id);
    }

    public async Task CancelRental(Booking booking)
    {
        await ChangeData(booking, x => booking.CancelRental());
    }

    public async Task ConfirmRental(Booking booking)
    {
        await ChangeData(booking, x => booking.ConfirmRental());
    }

    public async Task ChangeDate(Booking booking, DateTime newStartDate, DateTime newEndDate)
    {
        await ChangeData(booking, x => booking.ChangeDate(newStartDate, newEndDate));
    }

    public async Task<int> GetRoomNumber(Booking booking)
    {
        var room = await roomRepository.GetByIdAsync(booking.RoomId);
        if (room is null) throw new NullReferenceException("Room not found");
        return room.Number;
    }

    private async Task ChangeData(Booking booking, Action<Booking> changeBookingData)
    {
        changeBookingData(booking);
        await bookingRepository.UpdateAsync(booking);
    }
}