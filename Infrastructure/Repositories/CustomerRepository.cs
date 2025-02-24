using Domain.Entities;
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
    
    public async Task AddAsync(Customer customer)
    {
        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        context.Customers.Update(customer);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await GetByIdAsync(id);
        if (customer is null) return;
        context.Customers.Remove(customer);
        await context.SaveChangesAsync();
    }
}