using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(IEntity customer);
    Task DeleteBooking(IEntity customer, Guid bookingId);
    Task<Customer?> GetByEmail(string email);
    Task<Customer?> GetByPhoneNumber(string phoneNumber);
}