using AutoMapper;
using Leave_Management.Contracts;
using Leave_Management.Data;
using Leave_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {

        private readonly ILeaveAllocationRepository _AllocationRepo;
        private readonly ILeaveRequestRepository _LeaveRequestRepo;
        private readonly ILeaveTypeRepository _LeaveTypeRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;


        public LeaveRequestController(
            ILeaveRequestRepository LeaveRequestRepo,
            ILeaveTypeRepository LeavetypeRepo,
            ILeaveAllocationRepository AllocationRepo,
            IMapper mapper,
            UserManager<Employee> userManager
            )
        {
            _LeaveRequestRepo = LeaveRequestRepo;
            _LeaveTypeRepo = LeavetypeRepo;
            _AllocationRepo = AllocationRepo;
            _mapper = mapper;
            _userManager = userManager;

        }
        
        [Authorize(Roles ="Administrator")]
        // GET: LeaveRequestController1
        public ActionResult Index()
        {
            var leaverequests = _LeaveRequestRepo.Findall();

            var leaverequestsModel = _mapper.Map<List<LeaveRequestVM>>(leaverequests);
            var model = new AdminiLeaveRequestsViewVM
            {
                TotalRequests = leaverequestsModel.Count(),
                //ApprovedRequests = leaverequestsModel.Where(q=> q.Approved == true).Count(), //One or the other !!!
                ApprovedRequests = leaverequestsModel.Count(q => q.Approved == true),
                PendingRequests = leaverequestsModel.Count(q => q.Approved == null),
                RejectedRequests = leaverequestsModel.Count(q => q.Approved == false),
                LeaveRequests = leaverequestsModel
            };

            return View(model);
        }

        // GET: LeaveRequestController1/Details/5
        public ActionResult Details(int id)
        {
            var leaverequest = _LeaveRequestRepo.FindByID(id);
            var model = _mapper.Map<LeaveRequestVM>(leaverequest);

            return View(model);
        }

        public ActionResult MyLeave()
        {
            var employee = _userManager.GetUserAsync(User).Result;

            var leaveallocation = _AllocationRepo.GetLeaveAllocationsByEmployee(employee.Id);
            var leaveallocationModel = _mapper.Map<List<LeaveAllocationVM>>(leaveallocation);

            var leaverequest = _LeaveRequestRepo.GetLeaveRequestByEmployeeID(employee.Id);
            var leaverequestModel = _mapper.Map<List<LeaveRequestVM>>(leaverequest);

            var model = new MyLeaveVM
            {
                LeaveAllocations = leaveallocationModel,
                LeaveRequests = leaverequestModel
            };

            return View(model);
        }

        public ActionResult CancelRequest(int id)
        {
            var leaverequest = _LeaveRequestRepo.FindByID(id);
            _LeaveRequestRepo.Delete(leaverequest);

            return RedirectToAction("MyLeave");

        }

        // GET: LeaveRequestController1/Create
        public ActionResult Create()
        {
            var leavetypes = _LeaveTypeRepo.Findall();
            var leavetypeItems = leavetypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leavetypeItems
            };

            return View(model);
        }

        // POST: LeaveRequestController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestVM model)
        {
            try
            {
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);

                var leavetypes = _LeaveTypeRepo.Findall();
                var leavetypeItems = leavetypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });

                model.LeaveTypes = leavetypeItems;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (DateTime.Compare(startDate, endDate) > 0)
                {
                    ModelState.AddModelError("", "Start Date cannot be further in the future than the End Date");
                    return View(model);
                }

                var employee = _userManager.GetUserAsync(User).Result;
                var allocation = _AllocationRepo.GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);
                int daysRequested = (int)(endDate.Date - startDate.Date).TotalDays;

                if(daysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "You do NOT have sufficient days, get back to work lazy ass");
                    return View(model);
                }

                var leaveRequestModel = new LeaveRequestVM
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    LeaveTypeID = model.LeaveTypeId,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now
                };

                var leaverequest = _mapper.Map<LeaveRequest>(leaveRequestModel);
                var isSuccess = _LeaveRequestRepo.Create(leaverequest);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return View(model);

                }

                return RedirectToAction("MyLeave");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(model);
            }
        }

        public ActionResult ApproveRequest (int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaverequest = _LeaveRequestRepo.FindByID(id);
                var allocation = _AllocationRepo.GetLeaveAllocationsByEmployeeAndType(leaverequest.RequestingEmployeeId, leaverequest.Id);

                int daysRequested = (int)(leaverequest.EndDate.Date - leaverequest.StartDate.Date).TotalDays;
                
                //allocation.NumberOfDays = allocation.NumberOfDays - daysRequested;
                allocation.NumberOfDays -= daysRequested;

                //if (allocation.NumberOfDays < 0)
                //{
                //    ModelState.AddModelError("", "Something went wrong");
                //}

                leaverequest.Approved = true;
                leaverequest.ApprovedById = user.Id;
                leaverequest.DateActioned = DateTime.Now;

                _LeaveRequestRepo.Update(leaverequest);
                _AllocationRepo.Update(allocation);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {

                return RedirectToAction(nameof(Index)); 
            }

        }
        public ActionResult RejectRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaverequest = _LeaveRequestRepo.FindByID(id);
                leaverequest.Approved = false;
                leaverequest.ApprovedById = user.Id;
                leaverequest.DateActioned = DateTime.Now;

                var isSuccess = _LeaveRequestRepo.Update(leaverequest);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {

                return RedirectToAction(nameof(Index));
            }
        }
        // GET: LeaveRequestController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequestController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: LeaveRequestController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequestController1/Delete/5
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
