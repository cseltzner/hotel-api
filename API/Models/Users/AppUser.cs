using Microsoft.AspNetCore.Identity;

namespace API.Models.Users;

/// <summary>
/// Inherits from IdentityUser, allowing for access to properties such as Id, Email, Username, PasswordHash, etc.
/// </summary>
public class AppUser : IdentityUser
{
}