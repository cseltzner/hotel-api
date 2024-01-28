namespace API.Models;

// Note: MaxLength annotation not required because postgres TEXT column (default) has equal performance to VARCHAR
public class Guest
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
    public required string PhoneNumber { get; set; }

    public required ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}