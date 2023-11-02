﻿using HR.LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Domain;

public class LeaveAllocation : BaseEntity
{
    public string EmployeeId { get; set; } = string.Empty;
    public int NumberofDays { get; set; }
    public LeaveType? LeaveType { get; set; }
    public int  LeaveTypeID { get; set; }
    public int Period {  get; set; }

}
