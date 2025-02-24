namespace Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }

    private Customer()
    {
        
    }

    public Customer(string firstName, string lastName, string phoneNumber, string email)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void ChangeFirstName(string firstName)
    {
        FirstName = firstName;
    }

    public void ChangeLastName(string lastName)
    {
        LastName = lastName;
    }

    public void ChangePhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public void ChangeEmail(string email)
    {
        Email = email;
    }
}