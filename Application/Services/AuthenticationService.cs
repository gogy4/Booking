using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class AuthenticationService(CustomerServices customerService, IHttpContextAccessor httpContextAccessor)
{
    public async Task Register(string firstName, string lastName, string email, string phoneNumber, string password)
    {
        var hashedPassword = HashPassword(password);
        await customerService.CreateCustomer(firstName, lastName, email, phoneNumber, hashedPassword);
    }

    public async Task<bool> Login(string email, string password)
    {
        var customer = await customerService.GetByEmail(email);
        if (customer is null || customer.Password != HashPassword(password)) return false;
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, customer.FirstName),
            new(ClaimTypes.Email, customer.Email),
            new("CustomerId", customer.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties();

        await httpContextAccessor.HttpContext
            .SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        return true;
    }

    public async Task Logout()
    {
        await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private static string HashPassword(string password)
    {
        var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}