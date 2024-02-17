using API.Dto.Floor;
using API.Interfaces.Repositories;
using API.Models;
using API.Queries;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace API.Test.Controllers.FloorController;

public class GetFloorsTests
{
    private readonly List<Floor> testFloors;

    public GetFloorsTests()
    {
        testFloors = new List<Floor>
        {
            new Floor
            {
                Id = 1,
                FloorNumber = "1",
                Rooms = new List<Room>()
            },
            new Floor
            {
                Id = 2,
                FloorNumber = "2",
                Rooms = new List<Room>()
            },
            new Floor
            {
                Id = 3,
                FloorNumber = "3",
                Rooms = new List<Room>()
            }
        };
    }

    [Fact]
    public async void GetFloors_Returns200WithListOfFloorDto()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var mockRepo = Substitute.For<IFloorRepository>();
        var dummyQuery = new FloorQuery();
        mockRepo.GetFloors(dummyQuery).Returns(testFloors);
        var controller = new API.Controllers.FloorController(mockRepo, cache);

        // Act
        var response = await controller.GetFloors(dummyQuery);

        // Assert
        var expectedList = testFloors.Select(floor => new FloorDto()
        {
            Id = floor.Id,
            FloorNumber = floor.FloorNumber
        });

        var expectedResponse = new ObjectResult(expectedList)
        {
            StatusCode = 200
        };

        response.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async void GetFloors_AttemptsToPullDataFromCache()
    {
        // Arrange
        var cache = Substitute.For<IMemoryCache>();
        var mockRepo = Substitute.For<IFloorRepository>();
        var dummyQuery = new FloorQuery();
        var controller = new API.Controllers.FloorController(mockRepo, cache);

        // Act
        await controller.GetFloors(dummyQuery);

        // Assert
        cache.Received().Get($"floor:{dummyQuery.ToCacheKey()}");
    }

    [Fact]
    public async void GetFloors_SetsDataInCache()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var mockRepo = Substitute.For<IFloorRepository>();
        var dummyQuery = new FloorQuery();
        mockRepo.GetFloors(dummyQuery).Returns(testFloors);
        var controller = new API.Controllers.FloorController(mockRepo, cache);

        // Act
        await controller.GetFloors(dummyQuery);

        // Assert
        var cachedObject = cache.Get($"floor:{dummyQuery.ToCacheKey()}");
        cachedObject.Should().BeEquivalentTo(testFloors);
    }

    [Fact]
    public async void GetFloors_PullsDataFromCacheWhenAvailable()
    {
        // Arrange
        var cache = new MemoryCache(new MemoryCacheOptions());
        var mockRepo = Substitute.For<IFloorRepository>();
        var dummyQuery = new FloorQuery();

        cache.Set($"floor:{dummyQuery.ToCacheKey()}", testFloors); // Set value to cache, this value should be returned
        mockRepo.GetFloors(dummyQuery).Returns(new List<Floor>()); // Repo returns different value than cache, this value should not be returned

        var controller = new API.Controllers.FloorController(mockRepo, cache);

        // Act
        var response = await controller.GetFloors(dummyQuery);

        // Assert
        var expectedList = testFloors.Select(floor => new FloorDto()
        {
            Id = floor.Id,
            FloorNumber = floor.FloorNumber
        });

        var expectedResponse = new ObjectResult(expectedList)
        {
            StatusCode = 200
        };

        response.Should().BeEquivalentTo(expectedResponse);
    }
}