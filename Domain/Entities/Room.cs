using Domain.Enums;

namespace Domain.Entities;

public class Room
{
    public Guid Id { get; private set; }
    public RoomStatus Status { get; private set; }
    public int Number { get; private set; }
    public List<Guid> Customers { get; private set; }
    public int PricePerNight { get; private set; }
    public RoomType RoomType { get; private set; }

    private Room()
    {
    }

    public Room(int number, List<Guid> customerId, RoomType roomType, int pricePerNight)
    {
        Id = Guid.NewGuid();
        Number = number;
        Customers = new List<Guid>(customerId);
        Status = RoomStatus.Free;
        PricePerNight = pricePerNight;
        RoomType = roomType;
    }

    public void CancelRental()
    {
        if (Status != RoomStatus.Rental) throw new ArgumentException("Invalid status");
        Status = RoomStatus.Free;
    }

    public void PopulateRoom()
    {
        if (Status != RoomStatus.Free && Status != RoomStatus.Rental) throw new ArgumentException("Invalid status");
        Status = RoomStatus.Occupied;
    }

    public void RentalRoom()
    {
        if (Status != RoomStatus.Free) throw new ArgumentException("Invalid status");
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