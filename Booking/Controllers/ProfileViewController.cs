using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Booking.Models;

[Authorize]
public class ProfileViewController(
    CustomerServices customerServices,
    BookingServices bookingServices,
    RentalService rentalService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue("CustomerId");

        if (userId == null) return Unauthorized();

        var customer = await customerServices.GetById(Guid.Parse(userId));

        if (customer == null) return NotFound("Пользователь не найден");

        var bookings = new List<Domain.Entities.Booking>();

        foreach (var bookingId in customer.BookingIds)
        {
            var booking = await bookingServices.GetById(bookingId);
            if (booking != null) bookings.Add(booking);
        }

        ViewBag.Message = TempData["Message"];
        ViewBag.ErrorMessage = TempData["ErrorMessage"];

        return View(bookings);
    }

    [HttpGet]
    public async Task<IActionResult> EditUserData()
    {
        var userId = User.FindFirstValue("CustomerId");
        var customer = await customerServices.GetById(Guid.Parse(userId));
        if (customer == null)
        {
            TempData["ErrorMessage"] = "Пользователь не найден";
            return RedirectToAction("Index"); 
        }

        var model = new CustomerEditViewModel(customer);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditUserData(CustomerEditViewModel newCustomer)
    {
        var userId = Guid.Parse(User.FindFirstValue("CustomerId"));
        var customer = await customerServices.GetById(userId);

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Некорректные данные";
            return View(newCustomer);
        }

        try
        {
            await customerServices.EditCustomer(newCustomer, userId);
            TempData["Message"] = "Вы успешно изменили свои данные";
            return RedirectToAction("EditUserData"); 
        }
        catch (ArgumentException e)
        {
            TempData["ErrorMessage"] = e.Message;
            return View(newCustomer);
        }
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