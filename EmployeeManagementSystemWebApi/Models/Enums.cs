namespace EmployeeManagementSystemWebApi.Models;

public class Enums
{
    public enum AttendanceStatus
    {
        Present,
        Absent,
        Leave,
        HalfDay,
        Holiday
    }

    public enum LeaveType
    {
        Annual,
        Sick,
        Casual,
        Unpaid
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
