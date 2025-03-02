using System.Security.Cryptography;
using System.Text;
using Domain.Interfaces;

namespace Domain.Entities;

public class Customer : IEntity
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public List<Guid> BookingIds { get;   private set;} = new();
    
    public Customer(string firstName, string lastName, string phoneNumber, string email, string password)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        Password = password;
    }

    public void ChangeFirstName(string firstName) => FirstName = firstName;
    public void ChangeLastName(string lastName) => LastName = lastName;
    public void ChangePhoneNumber(string phoneNumber) => PhoneNumber = phoneNumber;
    public void ChangeEmail(string email) => Email = email;
    public void ChangePassword(string newPasswordHash) => Password = HashPassword(newPasswordHash);
    public void AddBooking(Guid bookingId) => BookingIds.Add(bookingId);
    public void RemoveBooking(Guid bookingId) => BookingIds.Remove(bookingId);
    
    private static string HashPassword(string password)
    {
        var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}