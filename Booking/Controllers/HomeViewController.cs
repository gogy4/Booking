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
}