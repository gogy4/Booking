using Domain.Entities;

namespace Application.Services;

public class RentalService(
    BookingServices bookingServices,
    ConfirmRentalService confirmRentalService,
    CancelRentalService cancelRentalService)
{
    public async Task<List<Room>> GetByDate(List<Room> rooms, DateTime startDate, DateTime? endDate = null)
    {
        var result = new List<Room>();
        foreach (var room in rooms)
        {
            var bookings = await GetBookings(room);
            if (await IsDateAvailable(bookings, startDate, endDate)) result.Add(room);
        }

        return result;
    }

    public async Task<List<string>> GetBookingDates(Room room)
    {
        var bookings = await GetBookings(room);
        var bookedDates = bookings
            .SelectMany(b => Enumerable.Range(0, (b.EndDate - b.StartDate).Days + 1)
                .Select(offset => b.StartDate.AddDays(offset).ToString("yyyy-MM-dd")))
            .ToList();

        return bookedDates;
    }

    private async Task<List<Booking?>> GetBookings(Room room)
    {
        var ids = room.BookingId;
        var bookings = new List<Booking?>();
        foreach (var id in ids) bookings.Add(await bookingServices.GetById(id));

        return bookings;
    }

    public async Task ConfirmRental(Room room, DateTime startDate, DateTime endDate)
    {
        await confirmRentalService.ConfirmRental(room, startDate, endDate);
    }

    public async Task CancelRental(Guid customerId, Guid bookingId)
    {
        await cancelRentalService.CancelRental(customerId, bookingId);
    }

    private static Task<bool> IsDateAvailable(List<Booking?> allBookings, DateTime startDate, DateTime? endDate = null)
    {
        endDate ??= startDate;

        return Task.FromResult(!allBookings.Any(b =>
            (startDate >= b.StartDate && startDate < b.EndDate.AddDays(1)) ||
            (endDate.Value > b.StartDate.AddDays(-1) && endDate.Value <= b.EndDate) |
            (startDate <= b.StartDate && endDate.Value >= b.EndDate)
        ));
    }
}