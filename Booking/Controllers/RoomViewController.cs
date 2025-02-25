using Application.Services;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class RoomViewController(RoomServices roomServices) : Controller
{
    [Route("{id}")]
    [HttpGet]
    public async Task<IActionResult> Index(Guid id)
    {
        var room = await roomServices.GetById(id);
        return View(new RoomDto(room));
    }
}