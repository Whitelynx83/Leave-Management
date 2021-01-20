﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Leave_Management.Models;

namespace Leave_Management.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<LeaveType> LeaveTypes {get; set; }

        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }

        public DbSet<Leave_Management.Models.LeaveRequestVM> LeaveRequestVM { get; set; }

        //public DbSet<Leave_Management.Models.LeaveTypeVM> LeaveTypeVM { get; set; }
        //public DbSet<Leave_Management.Models.EmployeeVM> EmployeeVM { get; set; }
        //public DbSet<Leave_Management.Models.LeaveAllocationVM> LeaveAllocationVM { get; set; }
        //public DbSet<Leave_Management.Models.EditLeaveAllocationVM> EditLeaveAllocationVM { get; set; }

    }
}
