using Leave_Management.Contracts;
using Leave_Management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Repository
{
    public class EmployeeRepository : IEmployeeRepository

    {
        private readonly ApplicationDbContext _db;

        /*
         Type "ctor" and then hit tab key twice : Will create the constructor
         */
        public EmployeeRepository(ApplicationDbContext db)
        {
            _db = db;

        }

        public bool Create(Employee entity)
        {
            _db.Employees.Add(entity);
            return Save();
        }

        public bool Delete(Employee entity)
        {
            _db.Employees.Remove(entity);
            return Save();
        }

        public ICollection<Employee> Findall()
        {
            return _db.Employees.ToList();
        }

        public Employee FindByID(int id)
        {
            return _db.Employees.Find(id);
        }

        public bool isExists(int id)
        {
            throw new Exception();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(Employee entity)
        {
            _db.Employees.Update(entity);
            return Save();
        }
    }
}
