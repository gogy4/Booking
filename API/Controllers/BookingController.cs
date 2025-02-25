using Application.Services;
using Dto;
using Domain.Enums;
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
    public async Task<IActionResult> Create([FromBody] BookingDto bookingDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        if (!bookingDto.IsValidDateRange())
        {
            return BadRequest("Start date must be earlier than end date.");
        }

        var booking = await bookingServices.CreateBooking(
            bookingDto.RoomId, 
            bookingDto.Customers, 
            bookingDto.StartDate, 
            bookingDto.EndDate, 
            bookingDto.Status
        );

        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }


    [HttpPatch("{id}/cancel-rental")]
    public async Task<IActionResult> CancelRental(Guid id)
    {
        var booking = await bookingServices.GetById(id);
        await bookingServices.CancelRental(booking);
        return NoContent();
    }

    [HttpPatch("{id}/confirm-rental")]
    public async Task<IActionResult> ConfirmRental(Guid id)
    {
        var booking = await bookingServices.GetById(id);
        await bookingServices.ConfirmRental(booking);
        return NoContent();
    }

    [HttpPut("{id}/change-date")]
    public async Task<IActionResult> ChangeDate(Guid id, [FromBody] ChangeDateDto dto)
    {
        var booking = await bookingServices.GetById(id);
        await bookingServices.ChangeDate(booking, dto.StartDate, dto.EndDate);
        return NoContent();
    }


    [HttpGet("search-by-status")]
    public async Task<IActionResult> GetBookings([FromQuery] BookingStatus status)
    {
        var bookings = await bookingServices.GetBookings(status);
        return Ok(bookings);
    }

    [HttpGet("search-by-dates")]
    public async Task<IActionResult> GetBookingsByDates([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate >= endDate) return BadRequest("Start date cannot be earlier than end date");
        var bookings = await bookingServices.GetBookingsByDate(startDate, endDate);
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