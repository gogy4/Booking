using System.Text.Json.Serialization;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

public class Room : IEntity
{
    public Guid Id { get; private set; }
    public List<Guid> BookingId { get; private set; }
    public RoomStatus Status { get; private set; }
    public int Number { get; private set; }
    public int PricePerNight { get; private set; }
    public RoomType RoomType { get; private set; }

    public Room()
    {
    }
    
    [JsonConstructor]
    public Room(int number, List<Guid> bookingId, int pricePerNight, RoomType roomType)
    {
        Id = Guid.NewGuid();
        Number = number;
        BookingId = bookingId;
        Status = RoomStatus.Free;
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