using System.Text.Json.Serialization;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Booking : IEntity
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get;  }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public BookingStatus Status { get;  }
        public Guid RoomId { get;  }
        
        public Booking() { }

        public Booking(Booking booking)
        {
            Id = Guid.NewGuid();
            CustomerId = booking.CustomerId;
            EndDate = booking.EndDate;
            StartDate = booking.StartDate;
            Status = booking.Status;
            RoomId = booking.RoomId;
        }
        
        [JsonConstructor]
        public Booking(Guid customer, DateTime startDate, DateTime endDate, Guid roomId)
        {
            Id = Guid.NewGuid();
            CustomerId = customer;
            StartDate = startDate;
            EndDate = endDate;
            RoomId = roomId;
        }

        public bool IsValidateDates()
        {
            return StartDate < EndDate;
        }
    }
}