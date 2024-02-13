using API.Dto.Floor;
using API.Interfaces.Repositories;
using API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace API.Test.Controllers.FloorController;

public class GetFloorTests
{
    [Fact]
    public async void GetFloor_Returns200StatusWithFloorDto()
    {
        // Arrange
        const int testFloorId = 1;
        var repositoryResponse = new Floor
        {
            Id = 1,
            FloorNumber = "2",
            Rooms = new List<Room>()
        };

        var cache = new MemoryCache(new MemoryCacheOptions());
        var mockFloorRepo = Substitute.For<IFloorRepository>();
        mockFloorRepo.GetFloorByFloorId(testFloorId).Returns(repositoryResponse);
        var controller = new API.Controllers.FloorController(mockFloorRepo, cache);

        // Act
        var result = await controller.GetFloor(testFloorId);

        // Assert
        var expectedResponse = new ObjectResult(new FloorDto
        {
            Id = 1,
            FloorNumber = "2"
        })
        {
            StatusCode = 200,
        };

        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async void GetFloor_Returns404_WhenFloorNotFound()
    {
        // Arrange
        const int testFloorId = 1;
        var cache = new MemoryCache(new MemoryCacheOptions());
        var mockFloorRepo = Substitute.For<IFloorRepository>();
        mockFloorRepo.GetFloorByFloorId(testFloorId).ReturnsNull(); // Repo returns null
        var controller = new API.Controllers.FloorController(mockFloorRepo, cache);

        // Act
        var response = await controller.GetFloor(testFloorId);

        // Assert
        var expectedResponse = new ObjectResult($"Floor with Id of {testFloorId} not found.")
        {
            StatusCode = 404
        };

        response.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async void GetFloor_AttemptsToPullDataFromCache()
    {
        // Arrange
        var mockFloorRepo = Substitute.For<IFloorRepository>();
        var mockCache = Substitute.For<IMemoryCache>();
        var controller = new API.Controllers.FloorController(mockFloorRepo, mockCache);

        // Act
        await controller.GetFloor(1);

        // Assert
        mockCache.Received().Get("floor:1");
    }

    [Fact]
    public async void GetFloor_SetsDataInCache()
    {
        // Arrange
        const int testFloorId = 1;

        var mockFloorRepo = Substitute.For<IFloorRepository>();
        var floorToPlaceInCache = new Floor
        {
            Id = testFloorId,
            FloorNumber = "123",
            Rooms = new List<Room>()
        };
        mockFloorRepo.GetFloorByFloorId(testFloorId).Returns(floorToPlaceInCache);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var controller = new API.Controllers.FloorController(mockFloorRepo, cache);

        // Act
        await controller.GetFloor(testFloorId);

        // Assert
        var cachedData = cache.Get($"floor:{testFloorId}");
        cachedData.Should().BeEquivalentTo(floorToPlaceInCache);
    }

    // GetFloor() should pull data from the cache, NOT the database when cache data is available
    [Fact]
    public async void GetFloor_PullsFromCache_WhenDataIsAvailable()
    {
        // Arrange
        const int testFloorId = 1;

        var mockFloorRepo = Substitute.For<IFloorRepository>();
        var floorInCache = new Floor
        {
            Id = testFloorId,
            FloorNumber = "123",
            Rooms = new List<Room>()
        };
        var floorInDatabase = new Floor
        {
            Id = 2,
            FloorNumber = "This should not be returned!",
            Rooms = new List<Room>()
        };

        mockFloorRepo.GetFloorByFloorId(testFloorId).Returns(floorInDatabase); // Return different value than the cache value

        var cache = new MemoryCache(new MemoryCacheOptions());
        cache.Set($"floor:{testFloorId}", floorInCache); // Set the floor in the cache

        var controller = new API.Controllers.FloorController(mockFloorRepo, cache);

        // Act
        var result = await controller.GetFloor(testFloorId);

        // Assert
        var expectedResult = new ObjectResult(new FloorDto()
        {
            Id = floorInCache.Id,
            FloorNumber = floorInCache.FloorNumber
        })
        {
            StatusCode = 200
        };

        var notExpectedResult = new ObjectResult(new FloorDto()
        {
            Id = floorInDatabase.Id,
            FloorNumber = floorInDatabase.FloorNumber
        })
        {
            StatusCode = 200
        };

        result.Should().BeEquivalentTo(expectedResult);
        result.Should().NotBeEquivalentTo(notExpectedResult);
    }
}