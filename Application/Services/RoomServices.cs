using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;

namespace Application.Services;

public class RoomServices(
    IRoomRepository roomRepository,
    BookingServices bookingServices)
{
    public async Task<Room> CreateRoom(int number, RoomType roomType, int pricePerNight)
    {
        var room = new Room(number, new List<Guid>(), pricePerNight, roomType);
        await roomRepository.AddAsync(room);
        return room;
    }

    public async Task<List<Room>> GetAll()
    {
        return await roomRepository.GetAllAsync();
    }

    public async Task<Room?> GetById(Guid roomId)
    {
        return await roomRepository.GetByIdAsync(roomId);
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