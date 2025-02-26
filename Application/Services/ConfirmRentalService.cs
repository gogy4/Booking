using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class ConfirmRentalService(
    IHttpContextAccessor httpContextAccessor,
    ICustomerRepository customerRepository,
    BookingServices bookingServices,
    IRoomRepository roomRepository)
{
    public async Task ConfirmRental(Room room, DateTime startDate, DateTime endDate)
    {
        var customerIdClaim = httpContextAccessor.HttpContext.User?.FindFirst("CustomerId")?.Value;
        if (customerIdClaim == null) throw new UnauthorizedAccessException("User is not logged in.");

        var customerId = Guid.Parse(customerIdClaim);

        var customer = await customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new Exception("Customer not found.");

        var booking = await bookingServices.CreateBooking(customerId, startDate, endDate, room.Id);
        await ConfirmRentalCustomer(customer, booking.Id);
        await ConfirmRentalRoom(room, booking.Id);
    }

    private async Task ConfirmRentalRoom(Room room, Guid bookingId)
    {
        await ChangeEntityData(room, r => room.RentalRoom(), roomRepository);
        if (!await roomRepository.HaveRoomAsync(room)) await roomRepository.AddAsync(room);
        await roomRepository.AddBookingAsync(room, bookingId);
    }

    private async Task ConfirmRentalCustomer(Customer customer, Guid bookingId)
    {
        await ChangeEntityData(customer, x => customer.AddBooking(bookingId), customerRepository);
    }

    private async Task ChangeEntityData<T>(T entity, Action<T> changeEntityData, IRepository<T> repository)
        where T : class, IEntity
    {
        if (entity is null) throw new KeyNotFoundException($"{typeof(T).Name} not found");

        changeEntityData(entity);
        await repository.UpdateAsync(entity);
    }
}