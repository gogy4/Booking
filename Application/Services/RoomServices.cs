using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;

namespace Application.Services;

public class RoomServices(IRoomRepository roomRepository, IBookingRepository bookingRepository, ICustomerRepository customerRepository)
{
    public async Task<Room> CreateRoom(int number, List<Guid> customerId, RoomType roomType, int pricePerNight)
    {
        var room = new Room(number, customerId, roomType, pricePerNight);
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
    
    public async Task CancelRental(Room room)
    {
        await ChangeDataRoom(room, r => room.CancelRental());
    }

    public async Task ConfirmRental(Room room)
    {
        await ChangeDataRoom(room, r => room.RentalRoom());
        await roomRepository.AddAsync(room);
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
        await ChangeDataRoom(room, r=>room.ChangePrice(newPrice));
    }

    private async Task ChangeDataRoom(Room room, Action<Room> changeRoomStatus)
    {
        if (room is null) throw new KeyNotFoundException("Room not found");
        changeRoomStatus(room);
        await roomRepository.UpdateAsync(room);
    }
}