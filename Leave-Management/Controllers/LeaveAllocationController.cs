using AutoMapper;
using Leave_Management.Contracts;
using Leave_Management.Data;
using Leave_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Controllers
{
    [Authorize(Roles = "Administrator")] //Redirect to Login when not authorized
    public class LeaveAllocationController : Controller
    {
        private readonly ILeaveAllocationRepository _Allocationrepo;
        private readonly ILeaveTypeRepository _Typerepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveAllocationController(
            ILeaveAllocationRepository Allocationrepo,
            ILeaveTypeRepository Typerepo,
            IMapper mapper,
            UserManager<Employee> userManager
            )
        {
            _Allocationrepo = Allocationrepo;
            _Typerepo = Typerepo;
            _mapper = mapper;
            _userManager = userManager;
        }
        

        // GET: LeaveAllocationController
        public ActionResult Index()
        {
            var leavetypes = _Typerepo.Findall().ToList();
            
            var mappedLeaveType = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes);

            var model = new CreateLeaveAllocationVM
            {
                LeaveTypes = mappedLeaveType,
                NumberUpdated = 0
            };

            return View(model);
        }

        public ActionResult SetLeave(int id)
        {
            var leavetype = _Typerepo.FindByID(id);
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;

            foreach (var emp in employees)
            {
                if (_Allocationrepo.CheckAllocation(id, emp.Id)) 
                    continue;

                var allocation = new LeaveAllocationVM
                {
                    DateCreated = DateTime.Now,
                    EmployeeId = emp.Id,
                    LeaveTypeId = id,
                    NumberOfDays = leavetype.DefaultDays,
                    Period = DateTime.Now.Year
                };
                
                var leaveallocation = _mapper.Map<LeaveAllocation>(allocation);
                _Allocationrepo.Create(leaveallocation);
                
            }
            return RedirectToAction(nameof(Index));

        }

        public ActionResult ListEmployees()
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var model = _mapper.Map<List<EmployeeVM>>(employees);
            return View(model);

        }

        // GET: LeaveAllocationController/Details/5
        public ActionResult Details(string id)
        {
            //var employee = _mapper.Map<EmployeeVM>(_userManager.FindByIdAsync(id).Result);
            //var allocations = _mapper.Map <List<LeaveAllocationVM>>(_Allocationrepo.GetLeaveAllocationsByEmployee(id));
            //var model = new ViewAllocationsVM
            //{
            //    Employee = employee,
            //    LeaveAllocations = allocations
            //};

            var employee = _userManager.FindByIdAsync(id).Result;
            employee.LeaveAllocations = _Allocationrepo.GetLeaveAllocationsByEmployee(id);
            var model = _mapper.Map<ViewAllocationsVM>(employee);

            return View(model);
        }

        // GET: LeaveAllocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Edit/5
        public ActionResult Edit(int id)
        {
            var leaveallocation = _Allocationrepo.FindByID(id);
            var model = _mapper.Map<EditLeaveAllocationVM>(leaveallocation);
            return View(model);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditLeaveAllocationVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record = _Allocationrepo.FindByID(model.Id);
                record.NumberOfDays = model.NumberOfDays;

                var isSuccess = _Allocationrepo.Update(record);
                
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong ...");
                    return View(model);
                }

                return RedirectToAction(nameof(Details), new {id = model.EmployeeId});
            }
            catch
            {
                return View(model);
            }
        }

        // GET: LeaveAllocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
