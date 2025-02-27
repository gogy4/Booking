using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController(RoomServices roomServices, RentalService rentalService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await roomServices.GetById(id);
        if (room == null) return NotFound("Room not found");
        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Room roomDto)
    {
        var room = await roomServices.CreateRoom(roomDto.Number, roomDto.RoomType, roomDto.PricePerNight);
        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPatch("{id}/confirm-rental")]
    public async Task<IActionResult> ConfirmRental(Guid id, DateTime startDate, DateTime endDate)
    {
        var room = await roomServices.GetById(id);
        await rentalService.ConfirmRental(room, startDate, endDate);
        return NoContent();
    }
    

    [HttpPatch("{id}/change-price")]
    public async Task<IActionResult> ChangePricePerNight(Guid id, [FromBody] int newPrice)
    {
        var room = await roomServices.GetById(id);
        await roomServices.ChangePricePerNight(room, newPrice);
        return NoContent();
    }
}