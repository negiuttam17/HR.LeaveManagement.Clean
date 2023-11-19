﻿using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Identity.DBContext
{
    public class HrLeaveManagementIDentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public HrLeaveManagementIDentityDbContext(DbContextOptions<HrLeaveManagementIDentityDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(HrLeaveManagementIDentityDbContext).Assembly);

        }
    }
}
