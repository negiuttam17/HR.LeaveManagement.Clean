using AutoMapper;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Command.CreateLeaveAllocation;

public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IUserService _userService;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    public CreateLeaveAllocationCommandHandler(IMapper mapper, 
        ILeaveAllocationRepository leaveAllocationRepository,
        ILeaveTypeRepository leaveTypeRepository, 
        IUserService userService)
    {
        _mapper = mapper;
        this._leaveAllocationRepository = leaveAllocationRepository;
        this._leaveTypeRepository = leaveTypeRepository;
        this._userService = userService;
    }
    public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if(validationResult.Errors.Any())
        {
            throw new BadRequestException("Invalid Leave Allocation Request", validationResult);
        }

        //Get Leave type for allocation
        var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeID);

        //GEt Employee
        var employees = await _userService.GetEmployees();

        //GEt Period
        var period = DateTime.Now.Year;

        //Assign Allocations IF allocation doesn't already exists for the period and leave type
        var allocations = new List<Domain.LeaveAllocation>();
        foreach(var emp in employees)
        {
            var allocationExists = await _leaveAllocationRepository.AllocationExists(emp.Id, request.LeaveTypeID, period);
            if(allocationExists == false)
            {
                allocations.Add(new Domain.LeaveAllocation
                {
                    EmployeeId = emp.Id,
                    LeaveTypeID = leaveType.Id,
                    NumberofDays = leaveType.DefaultDays,
                    Period = period,
                });
            }
        }
            
        if(allocations.Any())
        {
            await _leaveAllocationRepository.AddAllocation(allocations);
        }

        return Unit.Value;
    }
}
