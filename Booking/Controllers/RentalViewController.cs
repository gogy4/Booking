using Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace Booking.Controllers;

public class RentalViewController(RoomServices roomServices, RentalService rentalService) : Controller
{
    [HttpGet]
    public async Task<ActionResult> Index(Guid roomId, DateTime startDate, DateTime endDate)
    {
        var room = await roomServices.GetById(roomId);
        if (room == null) return NotFound("Комната не найдена");

        var bookedDates = (await rentalService.GetBookingDates(room)).Distinct().ToList();

        var model = new RentalViewModel
        {
            Room = room,
            StartDate = startDate,
            EndDate = endDate,
            BookedDates = bookedDates
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Rent(Guid roomId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var room = await roomServices.GetById(roomId);
            if (room == null) return NotFound("Комната не найдена");

            await rentalService.ConfirmRental(room, startDate, endDate);
            return RedirectToAction("Index", "HomeView");
        }
        catch (ArgumentException e)
        {
            return RedirectToAction(nameof(Index), "BookingView");
        }
    }
}