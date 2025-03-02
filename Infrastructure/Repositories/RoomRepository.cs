using Domain.Entities;
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

    public async Task AddAsync(Room room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        await context.Rooms.AddAsync(room);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(IEntity room)
    {
        if (room is not Room validRoom) throw new ArgumentException("Invalid room entity", nameof(room));
        context.Rooms.Update(validRoom);
        await context.SaveChangesAsync();
    }

    public async Task AddBookingAsync(Room room, Guid booking)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        room.AddBooking(booking);
        await UpdateAsync(room);
    }

    public async Task<bool> HaveRoomAsync(Room room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        return await context.Rooms.ContainsAsync(room);
    }

    public async Task DeleteBooking(IEntity room, Guid booking)
    {
        if (room is not Room validRoom) throw new ArgumentException("Invalid room entity", nameof(room));
        if (validRoom.BookingId.Contains(booking))
        {
            validRoom.BookingId.Remove(booking);
            await UpdateAsync(validRoom);
        }
    }
}