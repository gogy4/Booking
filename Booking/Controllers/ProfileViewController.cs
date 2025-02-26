using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize] 
public class ProfileViewController(CustomerServices customerServices, BookingServices bookingServices, RentalService rentalService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue("CustomerId"); 

        if (userId == null)
        {
            return Unauthorized();
        }

        var customer = await customerServices.GetById(Guid.Parse(userId));

        if (customer == null)
        {
            return NotFound("Пользователь не найден");
        }

        var bookings = new List<Domain.Entities.Booking>();

        foreach (var bookingId in customer.BookingIds)
        {
            var booking = await bookingServices.GetById(bookingId);
            if (booking != null)
            {
                bookings.Add(booking);
            }
        }

        ViewBag.Message = TempData["Message"];

        return View(bookings);
    }
    
    [HttpPost]
    public async Task<IActionResult> CancelRent(Guid bookingId)
    {
        var userId = User.FindFirstValue("CustomerId");
        
        try
        {
            await rentalService.CancelRental(Guid.Parse(userId), bookingId);
            TempData["Message"] = "Аренда успешно отменена!";
        }
        catch (Exception ex)
        {
            TempData["Message"] = ex.Message; 
        }

        return RedirectToAction("Index");
    }
}