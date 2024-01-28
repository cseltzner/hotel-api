namespace API.Models;

public class Floor
{
    public int Id { get; set; }
    public required string FloorNumber { get; set; }

    public required ICollection<Room> Rooms { get; set; } = new List<Room>();
}