using Domain.Entities;

public class RentalViewModel
{
    public Room Room { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<string> BookedDates { get; set; } 
}