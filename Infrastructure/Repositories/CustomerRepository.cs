using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await context.Customers.FindAsync(id);
    }

    public async Task<List<Customer?>> GetAllAsync()
    {
        return await context.Customers.ToListAsync();
    }

    public async Task<List<Customer>> GetCustomersAsync(List<Guid> ids)
    {
        var result = new List<Customer>();
        foreach (var id in ids)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer is null) throw new NullReferenceException("Customer not found");
            result.Add(customer);
        }

        return result;
    }


    public async Task<Customer?> GetByEmail(string email)
    {
        return await context.Customers.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<Customer?> GetByPhoneNumber(string phoneNumber)
    {
        return await context.Customers.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }

    public async Task AddAsync(Customer customer)
    {
        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(IEntity customer)
    {
        context.Customers.Update(customer as Customer);
        await context.SaveChangesAsync();
    }

    public async Task DeleteBooking(IEntity customer, Guid bookingId)
    {
        (customer as Customer)?.BookingIds.Remove(bookingId);
        await UpdateAsync(customer);
    }


    public async Task DeleteAsync(Guid id)
    {
        var customer = await GetByIdAsync(id);
        if (customer is null) return;
        context.Customers.Remove(customer);
        await context.SaveChangesAsync();
    }
}