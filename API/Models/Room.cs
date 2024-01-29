namespace API.Models;

public class Room
{
    public int Id { get; set; }
    public int RoomNumber { get; set; }

    public int FloorId { get; set; }
    public required Floor Floor { get; set; }

    public int RoomStatusId { get; set; }
    public required RoomStatus RoomStatus { get; set; }

    public int RoomClassId { get; set; }
    public required RoomClass RoomClass { get; set; }

    public required ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}