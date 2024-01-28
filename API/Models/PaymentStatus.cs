namespace API.Models;

public class PaymentStatus
{
    public int Id { get; set; }
    public required string CurrentPaymentStatus { get; set; }
}