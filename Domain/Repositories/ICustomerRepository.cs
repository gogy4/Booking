using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<List<Customer?>> GetAllAsync();
    Task<List<Customer>> GetCustomersAsync(List<Guid> ids);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Guid id);
}