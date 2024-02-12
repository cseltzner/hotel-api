using API.Context;
using API.Dto.Floor;
using API.Interfaces.Repositories;
using API.Models;
using API.Queries;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class FloorRepository : IFloorRepository
{
    private readonly HotelContext _context;

    public FloorRepository(HotelContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Floor>> GetFloors(FloorQuery query)
    {
        var floors = _context.Floors
            .AsQueryable();

        // Filter
        if (!string.IsNullOrWhiteSpace(query.FloorNumber))
        {
            floors = floors.Where(floor => floor.FloorNumber == query.FloorNumber);
        }

        // Sort
        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("FloorNumber", StringComparison.OrdinalIgnoreCase))
            {
                floors = query.IsDescending
                    ? floors.OrderByDescending(floor => floor.FloorNumber)
                    : floors.OrderBy(floor => floor.FloorNumber);
            }
        }

        // Pagination
        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await floors
            .Skip(skipNumber)
            .Take(query.PageSize)
            .ToListAsync();
    }

    public async Task<Floor?> GetFloorByFloorId(int floorId)
    {
        var floor = await _context.Floors
            .FindAsync(floorId);
        return floor;
    }

    public async Task<Floor?> GetFloorByFloorNum(string floorNum)
    {
        var floor = await _context.Floors
            .FirstOrDefaultAsync(floor => floor.FloorNumber == floorNum);
        return floor;
    }

    public async Task<Floor> CreateFloor(CreateFloorDto createFloorDto)
    {
        var newFloor = new Floor
        {
            FloorNumber = createFloorDto.FloorNumber,
            Rooms = new List<Room>()
        };

        var createdFloorEntity = await _context.Floors.AddAsync(newFloor);
        await _context.SaveChangesAsync();

        return createdFloorEntity.Entity;
    }

    public async Task<Floor?> UpdateFloor(int floorId, CreateFloorDto floorUpdateDto)
    {
        var floor = await _context.Floors
            .FindAsync(floorId);

        if (floor == null)
            return null;

        floor.FloorNumber = floorUpdateDto.FloorNumber;

        await _context.SaveChangesAsync();
        return floor;
    }

    public async Task<Floor?> DeleteFloor(int floorId)
    {
        var floor = await _context.Floors
            .FindAsync(floorId);

        if (floor == null)
            return null;

        _context.Floors.Remove(floor);
        await _context.SaveChangesAsync();

        return floor;
    }

    public async Task<bool> FloorExists(string floorNum)
    {
        return await _context.Floors.AnyAsync(floor => floor.FloorNumber == floorNum);
    }

    public async Task<bool> FloorExists(int floorId)
    {
        return await _context.Floors.AnyAsync(floor => floor.Id == floorId);
    }
}