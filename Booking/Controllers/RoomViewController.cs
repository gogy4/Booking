using Application.Services;
using Microsoft.AspNetCore.Mvc;

public class RoomViewController(RoomServices roomServices) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(Guid roomId)
    {
        var room = await roomServices.GetById(roomId);
        if (room is null) return NotFound();
        return View(room);
    }
}