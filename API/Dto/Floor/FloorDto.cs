using API.Models;

namespace API.Dto.Floor;

public class FloorDto
{
    public int Id { get; set; }
    public required string FloorNumber { get; set; }
}