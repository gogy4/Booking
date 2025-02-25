using Application.Services;
using Microsoft.AspNetCore.Mvc;
using EntityBooking = Domain.Entities.Booking;


namespace Booking.Controllers;

public class BookingViewController(BookingServices bookingServices, RoomServices roomServices) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var bookings = await bookingServices.GetBookings();
        var bookingsDto = new List<EntityBooking>();
        foreach (var booking in bookings)
        {
            var dto = new EntityBooking(booking);
            bookingsDto.Add(dto);
        }
        
        return View(bookingsDto);
    }
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var booking = await bookingServices.GetById(id);
        if (booking is null) return NotFound();
        return View(new EntityBooking(booking));
    }
}