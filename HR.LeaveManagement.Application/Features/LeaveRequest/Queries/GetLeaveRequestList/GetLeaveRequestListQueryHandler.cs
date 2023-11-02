using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList
{
    public class GetLeaveRequestListQueryHandler : IRequestHandler<GetLeaveRequestListQuery,
        List<LeaveRequestListDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        public GetLeaveRequestListQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListQuery request,
            CancellationToken cancellationToken)
        {
            //Check if it is logged in employee

            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestWithDetails();
            var requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequest);

            // Fill requests with employee information
            return requests;
        }
    }
}
