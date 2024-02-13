using API.Queries;
using FluentAssertions;

namespace API.Test.Queries;

public class FilterQueryTests
{
    [Fact]
    public void ToCacheKey_ReturnsMD5Hash()
    {
        // Arrange
        var filterQuery = new FilterQuery
        {
            SortBy = "test",
        };

        // Act
        var filterQueryHash = filterQuery.ToCacheKey();

        // Assert
        filterQueryHash.Should().MatchRegex("([a-fA-F\\d]{32})"); // md5 hash regex
    }

    [Fact]
    public void ToCacheKey_CreatesSameHash_IfObjectsAreTheSame()
    {
        // Arrange
        var filterQuery1 = new FilterQuery
        {
            SortBy = "test"
        };

        var filterQuery2 = new FilterQuery
        {
            SortBy = "test"
        };

        // Act
        var hash1 = filterQuery1.ToCacheKey();
        var hash2 = filterQuery2.ToCacheKey();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void ToCacheKey_CreatesDifferentHash_IfObjectsAreDifferent()
    {
        // Arrange
        var filterQuery1 = new FilterQuery
        {
            SortBy = "test 1"
        };

        var filterQuery2 = new FilterQuery
        {
            SortBy = "test 2"
        };

        // Act
        var hash1 = filterQuery1.ToCacheKey();
        var hash2 = filterQuery2.ToCacheKey();

        // Assert
        hash1.Should().NotBe(hash2);
    }
}