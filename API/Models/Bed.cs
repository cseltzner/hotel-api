namespace API.Models;

public class Bed
{
    public int Id { get; set; }
    public required string BedType { get; set; }

    public required ICollection<RoomClass> RoomClasses { get; set; } = new List<RoomClass>();
}