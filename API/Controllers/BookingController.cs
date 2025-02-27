using Application.Services;
using BookingEntity = Domain.Entities.Booking;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController(BookingServices bookingServices) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var booking = await bookingServices.GetById(id);
        if (booking is null) return NotFound();
        return Ok(booking);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingEntity booking)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!booking.IsValidateDates()) return BadRequest("Start date must be earlier than end date.");

        var newBooking = await bookingServices.CreateBooking(
            booking.CustomerId,
            booking.StartDate,
            booking.EndDate,
            booking.RoomId
        );

        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }


    [HttpGet("search-by-status")]
    public async Task<IActionResult> GetBookings([FromQuery] DateTime startDate)
    {
        var bookings = await bookingServices.GetBookings(startDate);
        return Ok(bookings);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(Guid id)
    {
        var booking = await bookingServices.GetById(id);
        await bookingServices.UpdateBooking(booking);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var booking = await bookingServices.GetById(id);
        await bookingServices.DeleteBooking(booking);
        return NoContent();
    }
}