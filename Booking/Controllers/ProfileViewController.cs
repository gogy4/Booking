/*using Application.Services;
using EntityBooking = Domain.Entities.Booking;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class ProfileViewController(BookingServices bookingServices, RoomServices roomServices) : Controller
{
    
    [HttpGet]
    public async Task<ActionResult> Index(Guid id)
    {
        var room = await roomServices.GetById(id);
        if (room == null)
        {
            return NotFound("Комната не найдена");
        }

        var bookings = room.BookingId;
        var currentBookingId = Guid.Empty;
        if (bookings != null) currentBookingId = bookings[0];
        if (currentBookingId == Guid.Empty) return NotFound("Не найдено аренды");
        var booking = await bookingServices.GetById(currentBookingId);
        if (booking == null) return NotFound("Не найдено аренды");

        var availableEndDate = await bookingServices.GetAvailableEndDate(id);
        TempData["AvailableEndDate"] = availableEndDate.ToString("yyyy-MM-dd");
        return View(new EntityBooking(booking)); 
    }
    


    [HttpPost]
    public async Task<IActionResult> CancelRental(Guid id)
    {
        try
        {
            var booking = await bookingServices.GetById(id);
            var room = await roomServices.GetById(booking.RoomId);
            await roomServices.CancelRental(room);
            await bookingServices.CancelRental(booking);
            return Ok("Вы успешно отменили аренду");
        }
        catch (ArgumentException e)
        {
            return RedirectToAction(nameof(Index), "BookingView");
        }
    }
}*/