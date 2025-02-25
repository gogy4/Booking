using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class RentalViewController(BookingServices bookingServices, RoomServices roomServices) : Controller
{
    [Route("rental-{roomId}")]
    [HttpGet]
    public async Task<ActionResult> Index(Guid roomId)
    {
        var room = await roomServices.GetById(roomId);
        if (room == null) return NotFound("Комната не найдена");
        return View(room);
    }

    [HttpPost]
    public async Task<IActionResult> Rent(Guid roomId, DateTime startDate, DateTime endDate, List<Guid> customers)
    {
        try
        {
            var room = await roomServices.GetById(roomId);
            if (room is null) return NotFound("Room not found");
            var booking = await bookingServices.CreateBooking(roomId, customers, startDate, endDate);
            
            await roomServices.ConfirmRental(room);
            return RedirectToAction("Index");
        }
        catch (ArgumentException e)
        {
            return RedirectToAction(nameof(Index), "BookingView");
        }
    }
}