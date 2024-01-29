namespace API.Models;

public class Feature
{
    public int Id { get; set; }
    public required string FeatureName { get; set; }

    public required ICollection<RoomClass> RoomClasses { get; set; } = new List<RoomClass>();
}