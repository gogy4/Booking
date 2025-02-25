using Domain.Enums;

namespace Booking.DTO;

public record class CreateRoomDto(int Number, List<Guid> Customers, RoomType RoomType, int PricePerNight);