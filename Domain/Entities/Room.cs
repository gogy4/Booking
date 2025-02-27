using System.Text.Json.Serialization;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

public class Room : IEntity
{
    public Guid Id { get; private set; }
    public List<Guid> BookingId { get; private set; }
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
        PricePerNight = room.PricePerNight;
        RoomType = room.RoomType;
    }

    public void ChangePrice(int newPrice)
    {
        PricePerNight = newPrice;
    }
}