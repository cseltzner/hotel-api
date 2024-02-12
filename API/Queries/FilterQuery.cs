namespace API.Queries;

/// <summary>
/// Base class for query objects to inherit. Includes sorting and pagination properties.
/// </summary>
public class FilterQuery
{
    public string? SortBy { get; set; } = null;
    public bool IsDescending { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Generates a unique cache string based on the query's properties.
    /// </summary>
    /// <returns>Unique cache string</returns>
    public string ToCacheKey()
    {
        var jsonString = System.Text.Json.JsonSerializer.Serialize(this);
        var md5HashBytes = System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes(jsonString));
        return Convert.ToHexString(md5HashBytes);
    }
}