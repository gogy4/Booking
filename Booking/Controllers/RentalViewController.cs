using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class RentalViewController(RoomServices roomServices, RentalService rentalService) : Controller
{
    [Route("rental-{roomId}")]
    [HttpGet]
    public async Task<ActionResult> Index(Guid roomId)
    {
        var room = await roomServices.GetById(roomId);
        if (room == null) return NotFound("Комната не найдена");

        var dates = await rentalService.GetBookingDates(room);
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
            await rentalService.ConfirmRental(room, startDate, endDate);
            return RedirectToAction("Index", "HomeView");
        }
        catch (ArgumentException e)
        {
            return RedirectToAction(nameof(Index), "BookingView");
        }
    }
}