﻿using Leave_Management.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management
{
    public static class SeedData
    {
        public static void Seed(
            UserManager<Employee> userManager,
            RoleManager<IdentityRole> roleManager) 
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        private static void SeedUsers(UserManager<Employee> userManager)
        {
            if(userManager.FindByNameAsync("Admin@localhost.com").Result == null)
            {
                var user = new Employee
                {
                    UserName = "Admin@localhost.com",
                    Email = "Admin@localhost.com"
                };
                var result = userManager.CreateAsync(user, "Test123$").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole { Name = "Administrator" };
                var result = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new IdentityRole { Name = "Employee" };
                var result = roleManager.CreateAsync(role).Result;
            }
        }

    }
}
