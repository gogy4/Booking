using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Enums;

namespace Dto;

public class BookingDto
{
    public Guid Id { get; set; }

    [Required] public Guid RoomId { get; set; }
    [Required] public List<Guid> Customers { get; set; } = new();
    
    [DataType(DataType.DateTime)] 
    public DateTime StartDate { get; set; }
    
    [DataType(DataType.DateTime)] 
    public DateTime EndDate { get; set; }

    [EnumDataType(typeof(BookingStatus))]
    public BookingStatus Status { get; set; }
    
    public int Number { get; set; }
    
    public bool IsValidDateRange() => StartDate < EndDate;


    public BookingDto(Booking booking, int number)
    {
        Id = booking.Id;
        RoomId = booking.RoomId;
        StartDate = booking.StartDate;
        Customers = booking.Customers;
        EndDate = booking.EndDate;
        Status = booking.Status;
        Number = number;
    }
}
