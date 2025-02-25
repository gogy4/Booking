﻿using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Enums;

namespace Dto;

public class RoomDto
{
    public Guid Id { get; private set; }

    [Required] public int Number { get; private set; }
    [Required] public List<Guid> Customers { get;private set; } = new();
    
    [Required] public RoomType RoomType { get; private set; }
    [Required] public RoomStatus Status { get; private set; }

    [Required] public int PricePerNight { get; private set; }

    public RoomDto(Room room)
    {
        Number = room.Number;
        RoomType = room.RoomType;
        PricePerNight = room.PricePerNight;
        Customers = room.Customers;
        Status = room.Status;
    }
}