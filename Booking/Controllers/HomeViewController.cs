using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class HomeViewController(RoomServices roomServices, BookingServices bookingServices) : Controller
{
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var rooms = await roomServices.GetAll();
        return View(rooms);
    }

    [HttpPost]
    public async Task<IActionResult> Search(DateTime startDate, DateTime? endDate = null)
    {
        var rooms = await roomServices.GetAll();
        var sortedRooms = await roomServices.GetByDate(rooms, startDate, endDate);
        return View("Index", sortedRooms); 
    }
}
