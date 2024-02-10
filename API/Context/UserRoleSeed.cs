using API.Identity;
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
                Name = IdentityRoles.Manager,
                NormalizedName = IdentityRoles.Manager.ToUpper()
            },

            // Employee has access to limited authorized routes
            // necessary for creating bookings and managing rooms
            new IdentityRole
            {
                Name = IdentityRoles.Employee,
                NormalizedName = IdentityRoles.Employee.ToUpper()
            },
        };
    }
}