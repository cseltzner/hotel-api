using API.Dto.Floor;
using API.Identity;
using API.Interfaces.Repositories;
using API.Models;
using API.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FloorController : ControllerBase
{
    private readonly IFloorRepository _floorRepository;
    private readonly IMemoryCache _cache;

    public FloorController(IFloorRepository floorRepository, IMemoryCache cache)
    {
        _floorRepository = floorRepository;
        _cache = cache;
    }

    ////////////
    // Routes //
    ////////////

    /// <summary>
    /// @route   GET /api/floor                 <br/>
    /// @desc    Get all floors                 <br/>
    /// @access  Public                         <br/>
    ///                                         <br/>
    /// @query   FloorQuery                     <br/>
    ///                                         <br/>
    /// @status  200 - returns List of FloorDto <br/>
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetFloors([FromQuery] FloorQuery query)
    {
        var floors = _cache.Get<ICollection<Floor>>($"floor:{query.ToCacheKey()}"); // Check cache

        // No cache entry found
        if (floors == null)
        {
            // Fetch from database
            floors = await _floorRepository.GetFloors(query);
            _cache.Set($"floor:{query.ToCacheKey()}", floors, TimeSpan.FromSeconds(10));
        }

        var floorDtos = floors.Select(floor => new FloorDto
        {
            Id = floor.Id,
            FloorNumber = floor.FloorNumber
        });

        return StatusCode(200, floorDtos);
    }


    /// <summary>
    /// @route   GET /api/floor/:floorId        <br/>
    /// @desc    Get floor by floorId           <br/>
    /// @access  Public                         <br/>
    ///                                         <br/>
    /// @status  200 - returns FloorDto         <br/>
    /// @status  404 - Floor not found          <br/>
    /// </summary>
    [HttpGet("{floorId:int}")]
    public async Task<IActionResult> GetFloor([FromRoute] int floorId)
    {
        bool needsCacheSet = false;

        // Get floor from cache
        var floor = _cache.Get<Floor>($"floor:{floorId}");
        if (floor == null)
        {
            // Get floor from database
            floor = await _floorRepository.GetFloorByFloorId(floorId);
            needsCacheSet = true;
        }

        // Cannot find floor in cache OR database
        if (floor == null)
        {
            return StatusCode(404, $"Floor with Id of {floorId} not found.");
        }

        if (needsCacheSet)
        {
            _cache.Set($"floor:{floorId}", floor, TimeSpan.FromSeconds(60));
        }

        var floorDto = new FloorDto
        {
            Id = floor.Id,
            FloorNumber = floor.FloorNumber
        };

        return StatusCode(200, floorDto);
    }

    /// <summary>
    /// @route   POST /api/floor                <br/>
    /// @desc    Create floor                   <br/>
    /// @access  Manager                        <br/>
    ///                                         <br/>
    /// @body    CreateFloorDto                 <br/>
    ///                                         <br/>
    /// @status  201 - returns FloorDto         <br/>
    /// @status  409 - Floor already exists     <br/>
    /// </summary>
    [HttpPost]
    // [Authorize(Roles = $"{IdentityRoles.Manager}")]
    public async Task<IActionResult> CreateFloor([FromBody] CreateFloorDto createFloorDto)
    {
        // Check if floor is in database
        if (await _floorRepository.FloorExists(createFloorDto.FloorNumber))
        {
            return StatusCode(409, $"Floor with floor number of {createFloorDto.FloorNumber} already exists.");
        }

        var createdFloor = await _floorRepository.CreateFloor(createFloorDto);
        _cache.Set($"floor:{createdFloor.Id}", createdFloor);
        _cache.Remove("floor:all");

        var createdFloorDto = new FloorDto
        {
            Id = createdFloor.Id,
            FloorNumber = createdFloor.FloorNumber
        };

        return StatusCode(201, createdFloorDto);
    }

    /// <summary>
    /// @route   PUT /api/floor/:floorId        <br/>
    /// @desc    Update floor                   <br/>
    /// @access  Manager                        <br/>
    ///                                         <br/>
    /// @status  200 - returns updated FloorDto <br/>
    /// @status  404 - Floor not found          <br/>
    /// </summary>
    [HttpPut("{floorId:int}")]
    // [Authorize(Roles = $"{IdentityRoles.Manager}")]
    public async Task<IActionResult> UpdateFloor([FromRoute] int floorId, [FromBody] CreateFloorDto updateFloorDto)
    {
        var updatedFloor = await _floorRepository.UpdateFloor(floorId, updateFloorDto);

        if (updatedFloor == null)
        {
            return StatusCode(404, $"Floor with Id of {floorId} not found.");
        }

        _cache.Set($"floor:{updatedFloor.Id}", updatedFloor);
        _cache.Remove("floor:all");

        var updatedFloorDto = new FloorDto
        {
            Id = updatedFloor.Id,
            FloorNumber = updatedFloor.FloorNumber
        };

        return StatusCode(200, updatedFloorDto);
    }

    /// <summary>
    /// @route   DELETE /api/floor/:floorId      <br/>
    /// @desc    Delete floor                   <br/>
    /// @access  Manager                        <br/>
    ///                                         <br/>
    /// @status  200 - returns deleted FloorDto <br/>
    /// @status  404 - Floor not found          <br/>
    /// </summary>
    [HttpDelete("{floorId:int}")]
    // [Authorize(Roles = $"{IdentityRoles.Manager}")]
    public async Task<IActionResult> DeleteFloor([FromRoute] int floorId)
    {
        var deletedFloor = await _floorRepository.DeleteFloor(floorId);

        if (deletedFloor == null)
        {
            return StatusCode(404, $"Floor with Id of {floorId} not found.");
        }

        _cache.Remove($"floor:{deletedFloor.Id}");
        _cache.Remove("floor:all");

        var deletedFloorDto = new FloorDto
        {
            Id = deletedFloor.Id,
            FloorNumber = deletedFloor.FloorNumber
        };

        return StatusCode(200, deletedFloorDto);
    }
}