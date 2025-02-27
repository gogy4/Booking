using System.Security.Cryptography;
using System.Text;
using Booking.Models;
using Domain.CustomerValidator;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class CustomerServices(ICustomerRepository customerRepository)
{
    public async Task<Customer> CreateCustomer(string firstName, string lastName, string email, string phoneNumber,
        string password)
    {
        var customer = new Customer(firstName, lastName, phoneNumber, email, password);
        await customerRepository.AddAsync(customer);
        return customer;
    }

    public async Task EditCustomer(CustomerEditViewModel customer, Guid customerId)
    {
        var validator = new CustomerEditValidator(customerRepository);
        var result = await validator.ValidateAsync(customer);
        if (!result.IsValid)
        {
            throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
        }
        
        if (customer.FirstName is not null) await ChangeFirstName(customerId, customer.FirstName);
        if (customer.LastName is not null) await ChangeLastName(customerId, customer.LastName);
        if (customer.PhoneNumber is not null) await ChangePhoneNumber(customerId, customer.PhoneNumber);
        if (customer.Email is not null)await ChangeEmail(customerId, customer.Email);
        if (customer.NewPassword is not null) await ChangePassword(customerId, customer.NewPassword);
    }

    public async Task<Customer?> GetById(Guid id)
    {
        return await customerRepository.GetByIdAsync(id);
    }

    public async Task ChangeFirstName(Guid customerId, string firstName)
    {
        await ChangeData(customerId, customer => customer.ChangeFirstName(firstName));
    }

    public async Task ChangeLastName(Guid customerId, string lastName)
    {
        await ChangeData(customerId, customer => customer.ChangeLastName(lastName));
    }

    public async Task ChangeEmail(Guid customerId, string email)
    {
        await ChangeData(customerId, customer => customer.ChangeEmail(email));
    }

    public async Task ChangePhoneNumber(Guid customerId, string phoneNumber)
    {
        await ChangeData(customerId, customer => customer.ChangePhoneNumber(phoneNumber));
    }

    public async Task ChangePassword(Guid customerId, string password)
    {
        await ChangeData(customerId, customer => customer.ChangePassword(password));
    }
    

    private async Task ChangeData(Guid customerId, Action<Customer> changeCustomerData)
    {
        var customer = await customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new KeyNotFoundException($"Customer with id: {customerId} does not exist");
        changeCustomerData(customer);
        await customerRepository.UpdateAsync(customer);
    }
}