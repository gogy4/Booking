using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class HomeViewController(RoomServices roomServices, RentalService rentalService) : Controller
{
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Index(DateTime? startDate = null, DateTime? endDate = null)
    {
        var rooms = await roomServices.GetAll();

        if (startDate.HasValue)
        {
            rooms = await rentalService.GetByDate(rooms, startDate.Value, endDate);
        }

        return View(rooms);
    }
}