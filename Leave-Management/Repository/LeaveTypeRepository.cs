using Leave_Management.Contracts;
using Leave_Management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        /*
         Type "ctor" and then hit tab key twice : Will create the constructor
         */
        public LeaveTypeRepository(ApplicationDbContext db)
        {
            _db = db;

        }

        
        public bool Create(LeaveType entity)
        {
            _db.LeaveTypes.Add(entity);
            return Save();

        }

        public bool Delete(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            return Save();
        }

        public ICollection<LeaveType> Findall()
        {
            return _db.LeaveTypes.ToList();
        }

        public LeaveType FindByID(int id)
        {
            return _db.LeaveTypes.Find(id);
        }

        public ICollection<LeaveType> GetEmployeesByLeaveType(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
          return _db.SaveChanges() > 0;
        }

        public bool Update(LeaveType entity)
        {
            _db.LeaveTypes.Update(entity);
            return Save();
        }
    }
}
