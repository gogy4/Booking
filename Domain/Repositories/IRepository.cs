using Domain.Interfaces;

namespace Infrastructure.Interfaces;

public interface IRepository<T> where T : IEntity
{
    Task UpdateAsync(IEntity entity);
    Task DeleteBooking(IEntity entity, Guid bookingId);
}