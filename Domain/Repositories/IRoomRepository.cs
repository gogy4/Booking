using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id);
    Task<List<Room?>> GetByStatusAsync(RoomStatus status = RoomStatus.All);
    Task<List<Room?>> GetByTypeAsync(RoomType roomType = RoomType.All);
    Task DeleteAsync(Guid id);
    Task AddAsync(Room room);
    Task UpdateAsync(Room room);
}