using API.Dto.Floor;
using API.Models;

namespace API.Interfaces.Repositories;

public interface IFloorRepository
{
    public Task<ICollection<Floor>> GetFloors();
    public Task<Floor?> GetFloorByFloorId(int floorId);
    public Task<Floor?> GetFloorByFloorNum(string floorNum);
    public Task<Floor> CreateFloor(CreateFloorDto createFloorDto);
    public Task<Floor?> UpdateFloor(int floorId, CreateFloorDto floorUpdateDto);
    public Task<Floor?> DeleteFloor(int floorId);
    public Task<bool> FloorExists(string floorNum);
    public Task<bool> FloorExists(int floorId);
}