using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequest.Command.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Command.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
    {
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<CreateLeaveRequestCommandHandler> _applogger;

        public CreateLeaveRequestCommandHandler(IEmailSender emailSender, IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository, ILeaveRequestRepository leaveRequestRepository,
            IAppLogger<CreateLeaveRequestCommandHandler> logger)
        {
            this._emailSender = emailSender;
            this._mapper = mapper;
            this._leaveRequestRepository = leaveRequestRepository;
            this._leaveTypeRepository = leaveTypeRepository;
            this._applogger = logger;
    }

        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid Leave Request", validationResult);

            //GEt Requesting employee id

            // check on employee's allocation

            // if allocations aren't enough, return validation error with message

            //Create Leave Request
            var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
            await _leaveRequestRepository.CreateAsync(leaveRequest);

            //send email confirmation

            try
            {
                var email = new EmailMessage
                {
                    To = String.Empty, /*Get Email from employee record*/
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D}" +
                          $"has been successfully submitted.",
                    Subject = "Leave Request Submitted"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {
                _applogger.LogWarning(ex.Message);
            }

            return Unit.Value;
        }
    }
}
