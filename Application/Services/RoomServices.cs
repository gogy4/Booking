using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;

namespace Application.Services;

public class RoomServices(
    IRoomRepository roomRepository,
    BookingServices bookingServices,
    ICustomerRepository customerRepository)
{
    public async Task<Room> CreateRoom(int number, List<Guid> customers, RoomType roomType, int pricePerNight)
    {
        var room = new Room(number, new List<Guid>(), customers, pricePerNight, roomType);
        await roomRepository.AddAsync(room);
        return room;
    }

    public async Task<List<Room>> GetAll()
    {
        return await roomRepository.GetAllAsync();
    }

    public async Task<List<Room>> GetByDate(List<Room> rooms, DateTime startDate, DateTime? endDate = null)
    {
        var result = new List<Room>();
        foreach (var room in rooms)
        {
            var bookings = await GetBookings(room);
            if (await bookingServices.IsDateAvailable(bookings, startDate, endDate)) result.Add(room);
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

    private async Task<List<Booking>> GetBookings(Room room)
    {
        var ids = room.BookingId;
        var bookings = new List<Booking>();
        foreach (var id in ids) bookings.Add(await bookingServices.GetById(id));

        return bookings;
    }

    public async Task<Room?> GetById(Guid roomId)
    {
        return await roomRepository.GetByIdAsync(roomId);
    }

    public async Task CancelRental(Room room)
    {
        await ChangeDataRoom(room, r => room.CancelRental());
    }

    public async Task ConfirmRental(Room room, DateTime startDate, DateTime endDate)
    {
        await ChangeDataRoom(room, r => room.RentalRoom());
        if (!await roomRepository.HaveRoomAsync(room)) await roomRepository.AddAsync(room);
        var booking = await bookingServices.CreateBooking(room.Customers, startDate, endDate);
        await roomRepository.AddBookingAsync(room, booking.Id);
    }

    public async Task PopulateRoom(Room room)
    {
        await ChangeDataRoom(room, r => room.PopulateRoom());
    }

    public async Task CleanRoom(Room room)
    {
        await ChangeDataRoom(room, r => room.CleanRoom());
    }

    public async Task SetFreeRoom(Room room)
    {
        await ChangeDataRoom(room, r => room.SetFreeRoom());
    }

    public async Task ChangePricePerNight(Room room, int newPrice)
    {
        await ChangeDataRoom(room, r => room.ChangePrice(newPrice));
    }

    private async Task ChangeDataRoom(Room room, Action<Room> changeRoomStatus)
    {
        if (room is null) throw new KeyNotFoundException("Room not found");
        changeRoomStatus(room);
        await roomRepository.UpdateAsync(room);
    }
}