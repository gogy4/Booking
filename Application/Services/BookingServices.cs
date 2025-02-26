using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;

namespace Application.Services;

public class BookingServices(
    IBookingRepository bookingRepository,
    ICustomerRepository customerRepository)
{
    public async Task<Booking> CreateBooking(List<Guid> customerIds, DateTime startDate, DateTime endDate)
    {
        var customers = await customerRepository.GetCustomersAsync(customerIds);
        if (customers is null) throw new NullReferenceException("Customers not found");
        if (startDate >= endDate) throw new ArgumentException("Start date cannot be earlier than end date");
        var booking = new Booking(customerIds, startDate, endDate);
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
    

    public async Task<bool> IsDateAvailable(List<Booking> allBookings, DateTime startDate, DateTime? endDate = null)
    {
        endDate ??= startDate; 

        return !allBookings.Any(b =>
                (startDate >= b.StartDate && startDate < b.EndDate.AddDays(1)) || 
                (endDate.Value > b.StartDate.AddDays(-1) && endDate.Value <= b.EndDate) |
                (startDate <= b.StartDate && endDate.Value >= b.EndDate) 
        );
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
        if (!await IsDateAvailable(await bookingRepository.GetAllAsync(), newStartDate, newEndDate)) throw new InvalidDataException("Those dates are already taken");
        await ChangeData(booking, x => booking.ChangeDate(newStartDate, newEndDate));
    }

    private async Task ChangeData(Booking booking, Action<Booking> changeBookingData)
    {
        changeBookingData(booking);
        await bookingRepository.UpdateAsync(booking);
    }
}