using Domain.Enums;

namespace Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; private set; }
        public Guid RoomId { get; private set; }
        public List<Guid> Customers { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public BookingStatus Status { get; private set; }
        
        private Booking() { }

        public Booking(Booking booking)
        {
            Id = Guid.NewGuid();
            RoomId = booking.RoomId;
            StartDate = booking.StartDate;
            EndDate = booking.EndDate;
            Customers = new List<Guid>();
            Status = booking.Status;
        }

        public Booking(Guid roomId, List<Guid> customers, DateTime startDate, DateTime endDate)
        {
            Id = Guid.NewGuid();
            RoomId = roomId;
            Customers = customers;
            StartDate = startDate;
            EndDate = endDate;
        }
        
        public void ChangeDate(DateTime newStartDate, DateTime newEndDate)
        {
            if (newStartDate >= newEndDate) throw new ArgumentException("Invalid date");
            StartDate = newStartDate;
            EndDate = newEndDate;
        }

        public void CancelRental()
        {
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }

        public void ConfirmRental()
        {
            if (StartDate < DateTime.Now) throw new ArgumentException("Cannot confirm an already past booking");
        }

        public bool IsValidateDates()
        {
            return StartDate < EndDate;
        }
    }
}