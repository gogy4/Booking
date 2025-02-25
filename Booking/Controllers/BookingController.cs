using Application.Services;
using Booking.DTO;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

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
    public async Task<IActionResult> Create([FromBody] CreateBookingDto bookingDto)
    {
        var booking = await bookingServices.CreateBooking(bookingDto.RoomId, bookingDto.Customers, bookingDto.StartDate,
            bookingDto.EndDate, bookingDto.Status);
        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }

    [HttpPatch("{id}/cancel-rental")]
    public async Task<IActionResult> CancelRental(Guid id)
    {
        await bookingServices.CancelRental(id);
        return NoContent();
    }

    [HttpPatch("{id}/confirm-rental")]
    public async Task<IActionResult> ConfirmRental(Guid id)
    {
        await bookingServices.ConfirmRental(id);
        return NoContent();
    }

    [HttpPatch("{id}/change-date")]
    public async Task<IActionResult> ChangeDate(Guid id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        await bookingServices.ChangeDate(id, startDate, endDate);
        return NoContent();
    }

    [HttpPatch("{id}/change-start-date")]
    public async Task<IActionResult> ChangeStartDate(Guid id, [FromQuery] DateTime startDate)
    {
        await bookingServices.ChangeStartDate(id, startDate);
        return NoContent();
    }

    [HttpPatch("{id}/change-end-date")]
    public async Task<IActionResult> ChangeEndDate(Guid id, [FromQuery] DateTime endDate)
    {
        await bookingServices.ChangeEndDate(id, endDate);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetBookings([FromQuery] BookingStatus status)
    {
        var bookings = await bookingServices.GetBookings(status);
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateBooking(Guid id)
    {
        await bookingServices.UpdateBooking(id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await bookingServices.DeleteBooking(id);
        return NoContent();
    }
}