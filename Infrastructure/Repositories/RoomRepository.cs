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

    public async Task<List<Room?>> GetByStatusAsync(RoomStatus status = RoomStatus.All)
    {
        if (status == RoomStatus.All) return await context.Rooms.ToListAsync();
        return await context.Rooms.Where(r => r.Status == status).ToListAsync();
    }

    public async Task<List<Room?>> GetByTypeAsync(RoomType roomType = RoomType.All)
    {
        if (roomType == RoomType.All) return await context.Rooms.ToListAsync();
        return await context.Rooms.Where(r => r.RoomType == roomType).ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var booking = await GetByIdAsync(id);
        if (booking is null) return;
        context.Rooms.Remove(booking);
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