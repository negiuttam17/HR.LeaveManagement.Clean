using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Identity.Models;
using HR.LeaveManagement.Persistence.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net.Quic;

namespace HR.LeaveManagement.Application.IntegrationTest
{
    public class HrDatabaseContextTests
    {
        private readonly IUserService userService;
        private HrDatabaseContext _hrDatabaseContext;

        public HrDatabaseContextTests(IUserService userService)
        {
            var dbOptions = new DbContextOptionsBuilder<HrDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.userService = userService;
            _hrDatabaseContext = new HrDatabaseContext(dbOptions, userService);
            
        }
        [Fact]
        public async void Save_SetDateCreatedValue()
        {
            //Arrange
            var leaveType = new LeaveType
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Test Vacation"
            };

            //Act
            await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            //Assert
            leaveType.DateCreated.ShouldNotBeNull();
        }

        [Fact]
        public async void Save_SetDateModifiedValue()
        {
            //Arrange
            var leaveType = new LeaveType
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Test Vacation"
            };

            //Act
            await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            //Assert
            leaveType.DateCreated.ShouldNotBeNull();
        }
    }
}