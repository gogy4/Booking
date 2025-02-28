using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Room : IEntity
    {
        public Guid Id { get; private set; }
        public List<Guid> BookingId { get;  }
        public int Number { get; private set; }
        public int PricePerNight { get; private set; }
        public RoomType RoomType { get; private set; }
        public string Description { get; private set; }  
        public string FullDescription { get; private set; }
        public string ImageUrl { get; private set; }  

        public Room()
        {
        }

        [JsonConstructor]
        public Room(int number, List<Guid> bookingId, int pricePerNight, RoomType roomType, string description, string imageUrl)
        {
            Id = Guid.NewGuid();
            Number = number;
            BookingId = bookingId;
            PricePerNight = pricePerNight;
            RoomType = roomType;
            Description = description;
            ImageUrl = imageUrl;
        }

        public void AddBooking(Guid bookingId)
        {
            BookingId.Add(bookingId);
        }

        public void ChangePrice(int newPrice)
        {
            PricePerNight = newPrice;
        }

        public void ChangeShortDescription(string shortDescription)
        {
            Description = shortDescription;
        }
        
        public void ChangeFullDescription(string fullDescription)
        {
            FullDescription = fullDescription;
        }

        public void ChangeImageUrl(string newImageUrl)
        {
            ImageUrl = newImageUrl;
        }
    }
}