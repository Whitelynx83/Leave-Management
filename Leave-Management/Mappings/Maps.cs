using AutoMapper;
using Leave_Management.Data;
using Leave_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<LeaveType, LeaveTypeVM>().ReverseMap();

            CreateMap<Employee, EmployeeVM>().ReverseMap();

            // Test with JF
            CreateMap<Employee, ViewAllocationsVM>()
                .ForMember(VM => VM.EmployeeId, q => q.MapFrom(E => E.Id))
                .ForMember(VM => VM.Employee, q => q.MapFrom(x => x));
                //.ForMember(VM => VM.LeaveAllocations, q => q.MapFrom(E => E.LeaveAllocations));


            CreateMap<LeaveAllocation, LeaveAllocationVM>().ReverseMap();
            CreateMap<LeaveAllocation, EditLeaveAllocationVM>().ReverseMap();

            /*
             Why didn't I have to map CreateLeaveAllocationVM and ViewAllocationsVM ???????????????????????????
             */

            CreateMap<LeaveRequest, LeaveRequestVM>().ReverseMap();
        }
    }
}
