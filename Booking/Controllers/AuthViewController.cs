using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers;

public class AuthViewController(AuthenticationService authenticationService) : Controller
{
    [HttpGet] 
    public async Task<IActionResult> Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            await authenticationService.Login(email, password);
            return RedirectToAction("Index", "HomeView");
        }
        catch (ArgumentException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return View();
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string firstName, string lastName, string email, string phoneNumber, string password)
    {
        try
        {
            await authenticationService.Register(firstName, lastName, email, phoneNumber, password);
            return RedirectToAction("Login");
        }
        catch (ArgumentException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await authenticationService.Logout();
        return RedirectToAction("Index", "HomeView");
    }
}