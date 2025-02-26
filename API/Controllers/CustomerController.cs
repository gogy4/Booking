using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController(CustomerServices customerServices) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateCustomer([FromBody] Customer customerDto)
    {
        var customer = await customerServices.CreateCustomer(customerDto.FirstName, customerDto.LastName,
            customerDto.Email,
            customerDto.PhoneNumber, customerDto.Password);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var customer = await customerServices.GetById(id);
        if (customer is null) return NotFound("Customer not found");

        return Ok(customer);
    }

    [HttpPatch("{id}/change-firstname")]
    public async Task<ActionResult> ChangeFirstName(Guid id, [FromBody] string firstName)
    {
        await customerServices.ChangeFirstName(id, firstName);
        return NoContent();
    }

    [HttpPatch("{id}/change-email")]
    public async Task<ActionResult> ChangeEmail(Guid id, [FromBody] string email)
    {
        await customerServices.ChangeEmail(id, email);
        return NoContent();
    }

    [HttpPatch("{id}/change-lastname")]
    public async Task<ActionResult> ChangeLastName(Guid id, [FromBody] string lastName)
    {
        await customerServices.ChangeLastName(id, lastName);
        return NoContent();
    }

    [HttpPatch("{id}/change-phonenumber")]
    public async Task<ActionResult> ChangePhoneNumber(Guid id, [FromBody] string phoneNumber)
    {
        await customerServices.ChangePhoneNumber(id, phoneNumber);
        return NoContent();
    }
}