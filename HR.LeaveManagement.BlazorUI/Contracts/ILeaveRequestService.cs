using HR.LeaveManagement.BlazorUI.Models.LeaveRequests;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.Contracts
{
    public interface ILeaveRequestService
    {
        Task<Response<Guid>> ApproveLeaveRequest(int id, bool approvalStatus);
        Task<Response<Guid>> CancelLeaveRequest(int id);
        Task<Response<Guid>> CreateLeaveRequest(LeaveRequestVM leaveRequest);
        //Task<AdminLeaveRequestViewVM> GetAdminLeaveRequests();
        Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList();
        Task<LeaveRequestVM> GetLeaveRequest(int id);
        Task<EmployeeLeaveRequestViewVM> GetUserLeaveRequests();
    }
}
