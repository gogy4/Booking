using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class CustomerServices(ICustomerRepository customerRepository)
{
    public async Task<Customer> CreateCustomer(string firstName, string lastName, string email, string phoneNumber)
    {
        var customer = new Customer(firstName, lastName, email, phoneNumber);
        await customerRepository.AddAsync(customer);
        return customer;
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


    private async Task ChangeData(Guid customerId, Action<Customer> changeCustomerData)
    {
        var customer = await customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new KeyNotFoundException($"Customer with id: {customerId} does not exist");
        changeCustomerData(customer);
        await customerRepository.UpdateAsync(customer);
    }
}