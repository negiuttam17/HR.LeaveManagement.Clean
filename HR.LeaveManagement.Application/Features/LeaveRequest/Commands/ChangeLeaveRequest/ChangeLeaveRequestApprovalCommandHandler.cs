using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Command.ChangeLeaveRequest
{
    public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<ChangeLeaveRequestApprovalCommand> _appLogger;

        public ChangeLeaveRequestApprovalCommandHandler(ILeaveRequestRepository leaveRequestRepository,
            ILeaveTypeRepository leaveTypeRepository, ILeaveAllocationRepository leaveAllocationRepository,IMapper mapper, IEmailSender emailSender,
            IAppLogger<ChangeLeaveRequestApprovalCommand> appLogger)
        {
            this._leaveRequestRepository = leaveRequestRepository;
            this._leaveTypeRepository = leaveTypeRepository;
            this._leaveAllocationRepository = leaveAllocationRepository;
            this._mapper = mapper;
            this._emailSender = emailSender;
            this._appLogger = appLogger;
        }
        public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            if (leaveRequest is null)
                throw new NotFoundException(nameof(leaveRequest), request.Id);

            leaveRequest.Approved = request.Approved;
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            //If request is approved, get and update the employee's allocation

            if(request.Approved)
            {
                int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                var allocation = await _leaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                allocation.NumberofDays -= daysRequested;

                await _leaveAllocationRepository.UpdateAsync(allocation);
            }
            //send email confirmatoin

            try
            {
                var email = new EmailMessage
                {
                    To = String.Empty, /*Get Email from employee record*/
                    Body = $"The approval status for your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D}" +
                          $"has been updated.",
                    Subject = "Leave Request Approval status updated"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {
                _appLogger.LogWarning(ex.Message);
            }

            return Unit.Value;
        }
    }
}
