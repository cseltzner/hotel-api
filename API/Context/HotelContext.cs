using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context;

public class HotelContext : DbContext
{
    public HotelContext(DbContextOptions<HotelContext> contextOptions) : base(contextOptions) {}

    public DbSet<Guest> Guests { get; set; }
    public DbSet<PaymentStatus> PaymentStatus { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Floor> Floors { get; set; }
    public DbSet<RoomClass> RoomClasses { get; set; }
    public DbSet<RoomStatus> RoomStatus { get; set; }
    public DbSet<Bed> Beds { get; set; }
    public DbSet<Feature> Features { get; set; }
}