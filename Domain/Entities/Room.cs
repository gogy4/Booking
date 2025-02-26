using System.Text.Json.Serialization;
using Domain.Enums;

namespace Domain.Entities;

public class Room
{
    public Guid Id { get; private set; }
    public List<Guid> BookingId { get; private set; }
    public RoomStatus Status { get; private set; }
    public int Number { get; private set; }
    public List<Guid> Customers { get; private set; }
    public int PricePerNight { get; private set; }
    public RoomType RoomType { get; private set; }

    public Room()
    {
    }
    
    [JsonConstructor]
    public Room(int number, List<Guid> bookingId, List<Guid> customers, int pricePerNight, RoomType roomType)
    {
        Id = Guid.NewGuid();
        Number = number;
        BookingId = bookingId ?? new List<Guid>(); 
        Status = RoomStatus.Free;
        Customers = customers ?? new List<Guid>();
        PricePerNight = pricePerNight;
        RoomType = roomType;
    }


    public void AddBooking(Guid bookingId)
    {
        BookingId.Add(bookingId);
    }

    public Room(Room room)
    {
        Id = Guid.NewGuid();
        BookingId = room.BookingId;
        Number = room.Number;
        Customers = new List<Guid>(room.Customers);
        Status = room.Status;
        PricePerNight = room.PricePerNight;
        RoomType = room.RoomType;
    }

    public void CancelRental()
    {
        Status = RoomStatus.Free;
    }

    public void PopulateRoom()
    {
        if (Status != RoomStatus.Free && Status != RoomStatus.Rental) throw new ArgumentException("Invalid status");
        Status = RoomStatus.Occupied;
    }

    public void RentalRoom()
    {
        Status = RoomStatus.Rental;
    }

    public void CleanRoom()
    {
        if (Status != RoomStatus.Occupied) throw new ArgumentException("Invalid status");
        Status = RoomStatus.Clean;
    }

    public void SetFreeRoom()
    {
        if (Status != RoomStatus.Clean) throw new ArgumentException("Invalid status");
        Status = RoomStatus.Free;
    }

    public void ChangePrice(int newPrice)
    {
        PricePerNight = newPrice;
    }
}