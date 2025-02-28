using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.CustomerValidator;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class AuthenticationService
{
    private readonly ICustomerRepository customerRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly CustomerServices customerServices;

    public AuthenticationService(ICustomerRepository customerRepository, IHttpContextAccessor httpContextAccessor, CustomerServices customerServices)
    {
        this.customerRepository = customerRepository;
        this.httpContextAccessor = httpContextAccessor;
        this.customerServices = customerServices;
    }

    public async Task Register(string firstName, string lastName, string email, string phoneNumber, string password)
    {
        var customer = new Customer(firstName, lastName, phoneNumber, email, password);
        var validator = new CustomerRegisterValidator(customerRepository);
        var result = await validator.ValidateAsync(customer);
        if (!result.IsValid)
        {
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
        }
        var hashedPassword = HashPassword(password);
        await customerServices.CreateCustomer(firstName, lastName, email, phoneNumber, hashedPassword);
    }

    public async Task Login(string email, string password)
    {
        var customer = await customerRepository.GetByEmail(email);
        if (customer is null) throw new ArgumentException("Customer not found");

        var newPassword = HashPassword(password);
        var validator = new CustomerLoginValidator(customerRepository);
        var result = await validator.ValidateAsync((email, newPassword));
        if (!result.IsValid)
        {
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, customer.FirstName),
            new(ClaimTypes.Email, customer.Email),
            new("CustomerId", customer.Id.ToString())
        };

        if (customer.Email == "admin@gmail.com")
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties();

        await httpContextAccessor.HttpContext
            .SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
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