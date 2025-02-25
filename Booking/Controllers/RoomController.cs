using Application.Services;
using Booking.DTO; // Добавляем DTO
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController(RoomServices roomServices) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await roomServices.GetById(id);
        if (room == null) return NotFound("Room not found");
        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomDto roomDto) // Используем DTO
    {
        var room = await roomServices.CreateRoom(roomDto.Number, roomDto.Customers, roomDto.RoomType, roomDto.PricePerNight);
        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPatch("{id}/cancel-rental")]
    public async Task<IActionResult> CancelRental(Guid id)
    {
        await roomServices.CancelRental(id);
        return NoContent();
    }
    
    [HttpPatch("{id}/populate-room")]
    public async Task<IActionResult> PopulateRoom(Guid id)
    {
        await roomServices.PopulateRoom(id);
        return NoContent();
    }
    
    [HttpPatch("{id}/clean-room")]
    public async Task<IActionResult> CleanRoom(Guid id)
    {
        await roomServices.CleanRoom(id);
        return NoContent();
    }
        
    [HttpPatch("{id}/set-free-room")]
    public async Task<IActionResult> SetFreeRoom(Guid id)
    {
        await roomServices.SetFreeRoom(id);
        return NoContent();
    } 
    
    [HttpPatch("{id}/change-price")]
    public async Task<IActionResult> ChangePricePerNight(Guid id, [FromBody] ChangePriceDto priceDto) // Исправлено
    {
        await roomServices.ChangePricePerNight(id, priceDto.NewPrice);
        return NoContent();
    } 
}