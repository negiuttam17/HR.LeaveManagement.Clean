﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Command.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommand : IRequest<Unit>
    {
          public int LeaveTypeID { get; set; }
    }
}
