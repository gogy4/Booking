using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id);
    Task<List<Room>> GetAllAsync();
    Task DeleteAsync(Room room);
    Task AddAsync(Room room);
    Task UpdateAsync(Room room);
}