using Domain.Enums;

namespace Booking.DTO;

public record class CreateBookingDto(Guid RoomId, List<Guid> Customers, DateTime StartDate, DateTime EndDate,
    BookingStatus Status);