using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class RentalViewController(RoomServices roomServices) : Controller
{
    [Route("rental-{roomId}")]
    [HttpGet]
    public async Task<ActionResult> Index(Guid roomId)
    {
        var room = await roomServices.GetById(roomId);
        if (room == null) return NotFound("Комната не найдена");

        var dates = await roomServices.GetBookingDates(room);
        ViewBag.BookedDates = dates;
        return View(room);
    }

    [HttpPost]
    public async Task<IActionResult> Rent(Guid roomId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var room = await roomServices.GetById(roomId);
            if (room is null) return NotFound("Room not found");
            await roomServices.ConfirmRental(room, startDate, endDate);
            return RedirectToAction("Index");
        }
        catch (ArgumentException e)
        {
            return RedirectToAction(nameof(Index), "BookingView");
        }
    }
}