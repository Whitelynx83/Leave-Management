using Leave_Management.Contracts;
using Leave_Management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository

    {
        private readonly ApplicationDbContext _db;

        /*
         Type "ctor" and then hit tab key twice : Will create the constructor
         */
        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;

        }

        public bool CheckAllocation(int leavetypeid, string employeeid)
        {
            var period = DateTime.Now.Year;

            return Findall()
                .Where(q => q.EmployeeId == employeeid && q.LeaveTypeId == leavetypeid && q.Period == period)
                .Any();
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public ICollection<LeaveAllocation> Findall()
        {
            //inner join example
            return _db.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .ToList();
        }

        public LeaveAllocation FindByID(int id)
        {
            var leaveallocation = _db.LeaveAllocations
                   .Include(q => q.LeaveType)
                   .Include(q => q.Employee)
                   .FirstOrDefault(q => q.Id == id);

            return leaveallocation;
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id)
        {
            return Findall()
                .Where(q => q.EmployeeId == id && q.Period == DateTime.Now.Year)
                .ToList();
        }

        public bool isExists(int id)
        {
            var exists = _db.LeaveAllocations.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}
