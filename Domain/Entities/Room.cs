using Domain.Enums;

namespace Domain.Entities;

public class Room
{
    public Guid Id { get; private set; }
    public RoomStatus Status { get; private set; }
    public Guid Number { get; private set; }
    public List<Guid> CustomerId { get; private set; }
    public int PricePerNight { get; private set; }
    public RoomType RoomType { get; private set; }

    private Room()
    {
    }

    public Room(Guid number, List<Guid> customerId, RoomType roomType, int pricePerNight)
    {
        Id = Guid.NewGuid();
        Number = number;
        CustomerId = new List<Guid>(customerId);
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