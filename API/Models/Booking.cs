namespace API.Models;

public class Booking
{
    public int Id { get; set; }
    public DateOnly CheckinDate { get; set; }
    public DateOnly CheckoutDate { get; set; }
    public int NumGuests { get; set; }
    public decimal? BookingTotal { get; set; }

    public int PaymentStatusId { get; set; }
    public required PaymentStatus PaymentStatus { get; set; }

    public int GuestId { get; set; }
    public required Guest Guest { get; set; }

    public required ICollection<Room> Rooms { get; set; } = new List<Room>();
}