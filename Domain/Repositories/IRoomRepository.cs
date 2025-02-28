using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Infrastructure.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    Task<Room?> GetByIdAsync(Guid id);
    Task<List<Room>> GetAllAsync();
    Task AddAsync(Room room);
    Task UpdateAsync(IEntity room);
    Task AddBookingAsync(Room room, Guid booking);
    Task<bool> HaveRoomAsync(Room room);
    Task DeleteBooking(IEntity room, Guid booking);
}