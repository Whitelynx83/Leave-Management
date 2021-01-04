using Leave_Management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Contracts
{
    public interface ILeaveTypeRepository : IRepositoryBase<LeaveType>
    {
        /*
         Specific function for ILeaveType
         */

        ICollection<LeaveType> GetEmployeesByLeaveType(int id);

    }
}
