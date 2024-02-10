using Microsoft.AspNetCore.Identity;

namespace API.Context;

public static class UserRoleSeed
{
    public static List<IdentityRole> SeedIdentityRoles()
    {
        return new List<IdentityRole>
        {
            // Manager has access to all authorized routes
            new IdentityRole
            {
                Name = "Manager",
                NormalizedName = "MANAGER"
            },

            // Employee has access to limited authorized routes
            // necessary for creating bookings and managing rooms
            new IdentityRole
            {
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            },
        };
    }
}