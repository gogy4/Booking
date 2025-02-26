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
        if (await authenticationService.Login(email, password)) return RedirectToAction("Index", "HomeView");
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string firstName, string lastName, string email, string phoneNumber, string password)
    {
        await authenticationService.Register(firstName, lastName, email, phoneNumber, password);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await authenticationService.Logout();
        return RedirectToAction("Index", "HomeView");
    }
}