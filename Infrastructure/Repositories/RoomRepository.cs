using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomRepository(AppDbContext context) : IRoomRepository
{
    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await context.Rooms.FindAsync(id);
    }

    public Task<List<Room>> GetAllAsync()
    {
        return context.Rooms.ToListAsync();
    }

    public async Task DeleteAsync(Room room)
    {
        if (room is null) return;
        context.Rooms.Remove(room);
        await context.SaveChangesAsync();
    }

    public async Task AddAsync(Room room)
    {
        await context.Rooms.AddAsync(room);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        context.Rooms.Update(room);
        await context.SaveChangesAsync();
    }
}