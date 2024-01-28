namespace API.Models;

public class RoomClass
{
    public int Id { get; set; }
    public decimal BasePrice { get; set; }

    public required ICollection<Feature> Features { get; set; } = new List<Feature>();
    public required ICollection<Bed> Beds { get; set; } = new List<Bed>();
}