using Infrastructure.Interfaces;

namespace Application.Services;

public class CancelRentalService(
    IBookingRepository bookingRepository,
    ICustomerRepository customerRepository,
    IRoomRepository roomRepository)
{
    public async Task CancelRental(Guid customerId, Guid bookingId)
    {
        var booking = await bookingRepository.GetByIdAsync(bookingId);
        var customer = await customerRepository.GetByIdAsync(customerId);
        var room = await roomRepository.GetByIdAsync(booking.RoomId);
        if (DateTime.Now >= booking.StartDate.AddDays(-7))
            throw new Exception("Вы не можете отменить аренду, если до нее осталось менее 7 дней.");
        await bookingRepository.DeleteBooking(booking, bookingId);
        await customerRepository.DeleteBooking(customer, bookingId);
        await roomRepository.DeleteBooking(room, bookingId);
    }
}