using Domain.Enums;

namespace Domain.Entities;

public class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public List<Guid> Customers { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    
    public BookingStatus Status { get; private set; }

    private Booking()
    {
        
    }

    public Booking(Guid roomId, List<Guid> customers, DateTime startDate, DateTime endDate, BookingStatus status)
    {
        RoomId = roomId;
        Customers = customers;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }

    public void CancelRental()
    {
        if (Status == BookingStatus.Free) throw new ArgumentException("Invalid status");
        Status = BookingStatus.Free;
    }

    public void ConfirmRental()
    {
        if (Status == BookingStatus.Booked) throw new ArgumentException("Invalid status");
        Status = BookingStatus.Booked;
    }

    public void ChangeDate(DateTime newStartDate, DateTime newEndDate)
    {
        StartDate = newStartDate;
        EndDate = newEndDate;
    }
    
    public void ChangeStartDate(DateTime newStartDate)
    {
        StartDate = newStartDate;
    }
    
    public void ChangeEndDate(DateTime newEndDate)
    {
        EndDate = newEndDate;
    }
}