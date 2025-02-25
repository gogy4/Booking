using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;

namespace Application.Services;

public class RoomServices(IRoomRepository roomRepository)
{
    public async Task<Room> CreateRoom(int number, List<Guid> customerId, RoomType roomType, int pricePerNight)
    {
        var room = new Room(number, customerId, roomType, pricePerNight);
        await roomRepository.AddAsync(room);
        return room;
    }

    public async Task<Room?> GetById(Guid roomId)
    {
        return await roomRepository.GetByIdAsync(roomId);
    }
    
    public async Task CancelRental(Guid roomId)
    {
        await ChangeDataRoom(roomId, room=>room.CancelRental());
    }

    public async Task ConfirmRental(Guid roomId)
    {
        await ChangeDataRoom(roomId, room => room.RentalRoom());
    }

    public async Task PopulateRoom(Guid roomId)
    {
       await ChangeDataRoom(roomId, room => room.PopulateRoom());
    }

    public async Task CleanRoom(Guid roomId)
    {
       await ChangeDataRoom(roomId, room => room.CleanRoom());
    }

    public async Task SetFreeRoom(Guid roomId)
    {
       await ChangeDataRoom(roomId, room => room.SetFreeRoom());
    }

    public async Task ChangePricePerNight(Guid roomId, int newPrice)
    {
        await ChangeDataRoom(roomId, room=>room.ChangePrice(newPrice));
    }

    private async Task ChangeDataRoom(Guid roomId, Action<Room> changeRoomStatus)
    {
        var room = await roomRepository.GetByIdAsync(roomId);
        if (room is null) throw new KeyNotFoundException("Room not found");
        changeRoomStatus(room);
        await roomRepository.UpdateAsync(room);
    }
}