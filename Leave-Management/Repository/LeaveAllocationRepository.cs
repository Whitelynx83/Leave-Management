﻿using Leave_Management.Contracts;
using Leave_Management.Data;
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
            return _db.LeaveAllocations.ToList();
        }

        public LeaveAllocation FindByID(int id)
        {
            return _db.LeaveAllocations.Find(id);
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
