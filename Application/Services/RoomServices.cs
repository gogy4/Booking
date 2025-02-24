using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;

namespace Application.Services;

public class RoomServices(IRoomRepository roomRepository)
{
    public async Task<Room> CreateRoom(Guid number, List<Guid> customerId, RoomType roomType, int pricePerNight)
    {
        var room = new Room(number, customerId, roomType, pricePerNight);
        await roomRepository.AddAsync(room);
        return room;
    }
    
    public async Task CancelRental(Guid roomId)
    {
        await ChangeStatusRoom(roomId, room=>room.CancelRental());
    }

    public async Task PopulateRoom(Guid roomId)
    {
       await ChangeStatusRoom(roomId, room => room.PopulateRoom());
    }

    public async Task CleanRoom(Guid roomId)
    {
       await ChangeStatusRoom(roomId, room => room.CleanRoom());
    }

    public async Task SetFreeRoom(Guid roomId)
    {
       await ChangeStatusRoom(roomId, room => room.SetFreeRoom());
    }

    public async Task ChangePricePerNight(Guid roomId, int newPrice)
    {
        await ChangeStatusRoom(roomId, room=>room.ChangePrice(newPrice));
    }

    private async Task ChangeStatusRoom(Guid roomId, Action<Room> changeRoomStatus)
    {
        var room = await roomRepository.GetByIdAsync(roomId);
        if (room is null) throw new KeyNotFoundException("Room not found");
        changeRoomStatus(room);
    }
}