using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;

namespace Application.Services;

public class BookingServices(
    IBookingRepository bookingRepository,
    IRoomRepository roomRepository,
    ICustomerRepository customerRepository)
{
    public async Task<Booking> CreateBooking(Guid roomId, List<Guid> customerIds, DateTime startDate, DateTime endDate)
    {
        var room = new Room(await roomRepository.GetByIdAsync(roomId));
        if (room is null) throw new NullReferenceException("Room not found");
        var customers = await customerRepository.GetCustomersAsync(customerIds);
        if (customers is null) throw new NullReferenceException("Customers not found");
        if (startDate >= endDate) throw new ArgumentException("Start date cannot be earlier than end date");
        var booking = new Booking(roomId, customerIds, startDate, endDate);
        await bookingRepository.AddAsync(booking);
        return booking;
    }

    public async Task<List<Booking>> GetBookings(DateTime startDate = default)
    {
        var bookings = await bookingRepository.GetAllAsync(startDate);
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
    
    public async Task<DateTime> GetAvailableEndDate(Guid id)
    {
        var allBookings = await bookingRepository.GetAllAsync();
    
        var nextBooking = allBookings
            .Where(b => b.StartDate > DateTime.Today)
            .OrderBy(b => b.StartDate)
            .FirstOrDefault();

        return nextBooking != null ? nextBooking.StartDate.AddDays(-1) : DateTime.Today.AddMonths(1);
    }
    
    public async Task<bool> IsDateAvailable(DateTime startDate, DateTime endDate)
    {
        var allBookings = await bookingRepository.GetAllAsync();

        return !allBookings.Any(b =>
            (startDate >= b.StartDate && startDate < b.EndDate) ||
            (endDate > b.StartDate && endDate <= b.EndDate) ||
            (startDate <= b.StartDate && endDate >= b.EndDate)
        );
    }

    public async Task<List<Booking>> SearchBookings(DateTime startDate, DateTime endDate)
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
        await bookingRepository.DeleteAsync(booking);
    }

    public async Task ConfirmRental(Booking booking, DateTime startDate, DateTime endDate)
    {
        await ChangeDate(booking, startDate, endDate);
        await ChangeData(booking, x => booking.ConfirmRental());
        await bookingRepository.AddAsync(booking);
    }

    public async Task ChangeDate(Booking booking, DateTime newStartDate, DateTime newEndDate)
    {
        if (!await IsDateAvailable(newStartDate, newEndDate)) throw new InvalidDataException("Those dates are already taken");
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