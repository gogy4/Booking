using Application.Services;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class BookingViewController(BookingServices bookingServices, RoomServices roomServices) : Controller
{
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var bookings = await bookingServices.GetBookings();
        var bookingsDto = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            var dto = new BookingDto(booking, await bookingServices.GetRoomNumber(booking));
            bookingsDto.Add(dto);
        }
        
        return View(bookingsDto);
    }
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var booking = await bookingServices.GetById(id);
        if (booking is null) return NotFound();
        return View(new BookingDto(booking, await bookingServices.GetRoomNumber(booking)));
    }
}