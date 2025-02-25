using Application.Services;
using Domain.Entities;
using Dto;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class RentalViewController(BookingServices bookingServices, RoomServices roomServices) : Controller
{
    [Route("rental-{id}")]
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
        return View(new BookingDto(booking, room.Number)); 
    }

    [HttpPost]
    public async Task<IActionResult> Rent(Guid id, DateTime startDate, DateTime endDate)
    {
        try
        {
            var booking = await bookingServices.GetById(id);
            await bookingServices.ChangeDate(booking, startDate, endDate);
            await bookingServices.ConfirmRental(booking);
            await roomServices.ConfirmRental(booking.RoomId);
            return RedirectToAction(nameof(Index), "BookingView");
        }
        catch (ArgumentException e)
        {
            return RedirectToAction(nameof(Index), "BookingView");
        }
    }


    [HttpPost]
    public async Task<IActionResult> CancelRental(Guid id)
    {
        try
        {
            var booking = await bookingServices.GetById(id);
            await bookingServices.CancelRental(booking);
            await roomServices.CancelRental(booking.RoomId);
            return Ok("Вы успешно отменили аренду");
        }
        catch (ArgumentException e)
        {
            return RedirectToAction(nameof(Index), "BookingView");
        }
    }
}