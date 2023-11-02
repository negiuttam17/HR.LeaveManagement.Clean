using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Command.CreateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Command.UpdateLeaveAllocation;

namespace HR.LeaveManagement.Application.MappingProfile
{
    public class LeaveAllocationProfile : Profile
    {
        public LeaveAllocationProfile()
        {
            CreateMap<LeaveAllocationDto, LeaveAllocationDto>().ReverseMap();
            CreateMap<LeaveAllocation, LeaveAllocationDto>();
            CreateMap<CreateLeaveAllocationCommand, LeaveAllocation>();
            CreateMap<UpdateLeaveAllocationCommandValidator, LeaveAllocation>();
        }
    }
}
