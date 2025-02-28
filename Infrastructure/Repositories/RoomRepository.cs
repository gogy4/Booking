using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
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

    public async Task UpdateAsync(IEntity room)
    {
        context.Rooms.Update(room as Room);
        await context.SaveChangesAsync();
    }

    public async Task AddBookingAsync(Room room, Guid booking)
    {
        room.AddBooking(booking);
        await UpdateAsync(room);
    }

    public async Task<bool> HaveRoomAsync(Room room)
    {
        return await context.Rooms.ContainsAsync(room);
    }

    public async Task DeleteBooking(IEntity room, Guid booking)
    {
        (room as Room)?.BookingId.Remove(booking);
        await UpdateAsync(room);
    }
}