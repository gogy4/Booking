using Domain.Entities;

namespace Booking.Models;

public class CustomerEditViewModel
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public string OldPassword { get; private set; }
    public string NewPassword { get; private set; }
    public string ConfirmPassword { get; private set; }

    public CustomerEditViewModel()
    {
        
    }

    public CustomerEditViewModel(Guid id, string firstName, string lastName, string phoneNumber, string email, string oldPassword, string newPassword, string confirmPassword)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        OldPassword = oldPassword;
        NewPassword = newPassword;
        ConfirmPassword = confirmPassword;
    }

    public CustomerEditViewModel(Customer customer)
    {
        Id = customer.Id;
        FirstName = customer.FirstName;
        LastName = customer.LastName;
        PhoneNumber = customer.PhoneNumber;
        Email = customer.Email;
        OldPassword = customer.Password;
    }
}