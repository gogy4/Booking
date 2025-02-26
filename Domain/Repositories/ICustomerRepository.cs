using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<List<Customer?>> GetAllAsync();
    Task<List<Customer>> GetCustomersAsync(List<Guid> ids);
    Task AddAsync(Customer customer);
    Task UpdateAsync(IEntity customer);
    Task DeleteAsync(Guid id);
    Task DeleteBooking(IEntity customer, Guid bookingId);
    
    Task<Customer?> GetByEmail(string email);
}