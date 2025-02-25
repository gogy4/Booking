using Application.Services;
using EntityBooking = Domain.Entities.Booking;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class ProfileViewController(BookingServices bookingServices, RoomServices roomServices) : Controller
{
    
    [HttpGet]
    public async Task<ActionResult> Index(Guid id)
    {
        var booking = await bookingServices.GetById(id);
        if (booking == null) return NotFound("Не найдено объявление");
        var room = await roomServices.GetById(booking.RoomId); 
        if (room == null)
        {
            return NotFound("Комната не найдена");
        }
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
}